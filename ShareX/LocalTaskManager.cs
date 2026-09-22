#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib;
using ShareX.Tools;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareX
{
    /// <summary>
    /// Entry points that turn captures, files and dropped data into local tasks.
    /// This is the CrySnap replacement for the upload manager: nothing here leaves the machine.
    /// </summary>
    public static class LocalTaskManager
    {
        public static void RunFileTask(string filePath, TaskSettings taskSettings = null)
        {
            if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    WorkerTask task = WorkerTask.CreateFileTask(filePath, taskSettings);
                    TaskManager.Start(task);
                }
                else if (Directory.Exists(filePath))
                {
                    string[] files = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories);
                    RunFileTask(files, taskSettings);
                }
            }
        }

        public static void RunFileTask(string[] files, TaskSettings taskSettings = null)
        {
            if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    RunFileTask(file, taskSettings);
                }
            }
        }

        public static void RunImageTask(Bitmap bmp, TaskSettings taskSettings, bool skipQuickTaskMenu = false, bool skipAfterCaptureWindow = false)
        {
            TaskMetadata metadata = new TaskMetadata(bmp);
            RunImageTask(metadata, taskSettings, skipQuickTaskMenu, skipAfterCaptureWindow);
        }

        public static void RunImageTask(TaskMetadata metadata, TaskSettings taskSettings, bool skipQuickTaskMenu = false, bool skipAfterCaptureWindow = false)
        {
            if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

            if (metadata != null && metadata.Image != null && taskSettings != null)
            {
                if (!skipQuickTaskMenu && taskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowQuickTaskMenu))
                {
                    QuickTaskMenu quickTaskMenu = new QuickTaskMenu();

                    quickTaskMenu.TaskInfoSelected += taskInfo =>
                    {
                        if (taskInfo == null)
                        {
                            RunImageTask(metadata, taskSettings, true);
                        }
                        else if (taskInfo.IsValid)
                        {
                            taskSettings.AfterCaptureJob = taskInfo.AfterCaptureTasks;
                            RunImageTask(metadata, taskSettings, true);
                        }
                    };

                    quickTaskMenu.ShowMenu();

                    return;
                }

                void StartImageTask(string customFileName)
                {
                    WorkerTask task = WorkerTask.CreateImageTask(metadata, taskSettings, customFileName);
                    TaskManager.Start(task);
                }

                if (!skipAfterCaptureWindow && taskSettings.AfterCaptureJob.HasFlag(AfterCaptureTasks.ShowAfterCaptureWindow))
                {
                    TaskHelpers.ShowAfterCaptureWindow(taskSettings, result =>
                    {
                        if (result.Accepted)
                        {
                            StartImageTask(result.FileName);
                        }
                    }, metadata);

                    return;
                }

                StartImageTask(null);
            }
        }

        public static void RunTextTask(string text, TaskSettings taskSettings = null)
        {
            if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

            if (!string.IsNullOrEmpty(text))
            {
                WorkerTask task = WorkerTask.CreateTextTask(text, taskSettings);
                TaskManager.Start(task);
            }
        }

        public static void HandleDroppedData(IDataObject data, TaskSettings taskSettings = null)
        {
            if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

            if (data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = data.GetData(DataFormats.FileDrop, false) as string[];
                RunFileTask(files, taskSettings);
            }
            else if (data.GetDataPresent(DataFormats.Bitmap, false))
            {
                Bitmap bmp = data.GetData(DataFormats.Bitmap, false) as Bitmap;
                RunImageTask(bmp, taskSettings);
            }
            else if (data.GetDataPresent(DataFormats.Text, false))
            {
                string text = data.GetData(DataFormats.Text, false) as string;
                RunTextTask(text, taskSettings);
            }
        }

        public static void IndexFolder(TaskSettings taskSettings = null)
        {
            string selectedPath = FileHelpers.BrowseFolder();

            if (!string.IsNullOrEmpty(selectedPath))
            {
                IndexFolder(selectedPath, taskSettings);
            }
        }

        public static void IndexFolder(string folderPath, TaskSettings taskSettings = null)
        {
            if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
            {
                if (taskSettings == null) taskSettings = TaskSettings.GetDefaultTaskSettings();

                taskSettings.ToolsSettings.IndexerSettings.BinaryUnits = Program.Settings.BinaryUnits;

                string source = null;

                Task.Run(() =>
                {
                    source = Indexer.Index(folderPath, taskSettings.ToolsSettings.IndexerSettings);
                }).ContinueInCurrentContext(() =>
                {
                    if (!string.IsNullOrEmpty(source))
                    {
                        WorkerTask task = WorkerTask.CreateTextTask(source, taskSettings);
                        task.Info.FileName = Path.ChangeExtension(task.Info.FileName, taskSettings.ToolsSettings.IndexerSettings.Output.ToString().ToLower());
                        TaskManager.Start(task);
                    }
                });
            }
        }
    }
}
