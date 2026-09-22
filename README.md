# CrySnap

CrySnap is a personal, **local-only** fork of [ShareX](https://github.com/ShareX/ShareX).
It keeps the parts of ShareX that work entirely on your own machine and removes everything
that talks to the internet. All credit for the application goes to the ShareX Team.

It is licensed under the **GNU General Public License v3**, same as ShareX. See `LICENSE.txt`.

## What it does

- Screen capture: region, window, monitor, fullscreen, scrolling capture, auto capture.
- Screen recording to MP4 and GIF (ffmpeg is bundled with the installer).
- Built-in image editor, image effects, beautifier, pin to screen, color picker, ruler, OCR
  (Windows local OCR engine), QR code, hash checker, metadata viewer and the other local tools.
- Saves to `Documents\CrySnap\Screenshots`, copies to clipboard, opens in editor, runs your own
  external actions. History and thumbnails are local files only.
- Global hotkeys, tray icon, workflows and per-task settings, exactly like ShareX.

## What was removed

CrySnap contains **no network code**. Compared with ShareX it has no:

- Uploaders or destinations of any kind (image hosts, file hosts, FTP/SFTP, S3, custom uploaders).
- URL shorteners, URL sharing, social sharing, clipboard/URL upload, drag-and-drop upload window.
- Browser extension bridge (native messaging host), Steam or Microsoft Store builds.
- Auto-update. The app never checks GitHub or any other server.
- Cloud AI image analysis (OpenAI/Gemini/OpenRouter), reverse image search, proxy settings.
- ffmpeg downloader. ffmpeg ships inside the installer instead.

The only URLs left in the binary are opened in your browser on explicit request from the About
window and a couple of help buttons. The application itself never makes an HTTP request.

## Download / Install

Grab the latest installer from the releases page:

**https://github.com/crymetr/cryshare/releases/latest**

Under **Assets**, download and run `CrySnap-<version>-setup-x64.exe`. It installs to its own
`Program Files\CrySnap` and stores settings in `Documents\CrySnap`, so it never touches an
existing ShareX install. Prefer no installer? Use the `-portable-x64.zip` instead.

The installer is unsigned, so Windows SmartScreen shows a warning the first time. Click
**More info -> Run anyway**.

## Building locally

Requires the .NET 10 SDK and (for the installer) Inno Setup 6.

```powershell
dotnet build ShareX.sln -c Release -p:Platform=x64
# app: ShareX\bin\Release\win-x64\CrySnap.exe
```

To build the installer, put `ffmpeg.exe` into the `Tools` folder first (CI does this
automatically), then run `ShareX.Setup.exe -job Release -platform x64`.

### Cutting a release

1. Bump `<Version>` in `Directory.build.props` (optional, CI overrides it from the tag).
2. `git tag vX.Y.Z && git push origin vX.Y.Z`
3. CI builds and publishes the setup and portable zip as a GitHub Release.

## Upstream

To pull in new ShareX changes, add the upstream remote and merge. Expect conflicts in the
files where upload code was removed.

```powershell
git remote add upstream https://github.com/ShareX/ShareX.git
git fetch upstream
git merge upstream/develop
```
