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

namespace ShareX.HelpersLib
{
    /// <summary>
    /// The only URLs the app knows about. They are opened in the system browser on explicit user request (About window, help buttons).
    /// The app itself never contacts these hosts.
    /// </summary>
    public static class Links
    {
        public const string GitHub = "https://github.com/crymetr/crysnap";
        public const string GitHubIssues = GitHub + "/issues";
        public const string License = GitHub + "/blob/main/LICENSE.txt";

        public const string UpstreamWebsite = "https://getsharex.com";
        public const string UpstreamGitHub = "https://github.com/ShareX/ShareX";
        public const string Jaex = "https://github.com/Jaex";
        public const string McoreD = "https://github.com/McoreD";

        public const string ImageEffects = UpstreamWebsite + "/image-effects";
        public const string Actions = UpstreamWebsite + "/actions";
        private const string Docs = UpstreamWebsite + "/docs";
        public const string DocsKeybinds = Docs + "/keybinds";
        public const string DocsOCR = Docs + "/ocr";
        public const string DocsScrollingScreenshot = Docs + "/scrolling-screenshot";
    }
}
