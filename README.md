# MixedReality-Samples

## Overview

Companion source code for Dawid Borycki's *Programming for Mixed Reality with Windows 10, Unity, Vuforia, and UrhoSharp* (Microsoft Press, 2018). The chapter-based projects explore UWP application development, media and device integration, and interactive 3D experiences for HoloLens and Windows Mixed Reality.

> This repository is retained as a historical technical example. It reflects the development environment and APIs used when the accompanying book was originally published.

## Technical focus

- C#/.NET, UWP, XAML, and data binding
- Camera capture, speech interaction, and device communication
- Unity scenes, physics, animation, and spatial interaction
- HoloToolkit spatial mapping and input handling
- Vuforia image targets and UrhoSharp
- DirectX and SharpDX introductory projects

## Repository workflow

Each chapter contains independent projects rather than components of one application. `Common/MixedReality.Common` provides shared UWP helpers and view models.

| Location | Focus |
| --- | --- |
| `Chapter_02`–`Chapter_03` | Development tools and initial holographic/UWP applications |
| `Chapter_04`–`Chapter_07` | UWP interfaces, media, vision-service integration, and device communication |
| `Chapter_09`–`Chapter_11` | Unity scenes, physics, animation, and navigation |
| `Chapter_12`–`Chapter_13` | HoloLens interaction, spatial mapping, and a 3D vision assistant |
| `Chapter_14` | Attaching virtual content to image targets with Vuforia |
| `Chapter_15` | UrhoSharp interaction, physics, and spatial mapping |

## Getting started

### Original development environment

The solution was created with Visual Studio 2017 on Windows 10. The inspected UWP projects target Windows SDK `10.0.16299.0`. Most Unity projects record Unity `2017.3.1f1`; `Chapter_02/UnityApp` records `2017.2.0f3`. Check the selected project's `ProjectSettings/ProjectVersion.txt` and project file before opening it.

1. Clone or download the repository. It includes bundled Unity assets and generated editor caches, so the checkout is relatively large.
2. For UWP examples, install the original Visual Studio UWP development tools and matching Windows SDK, then open `MixedReality-Samples.sln`. Select the chapter project you want to run; `Chapter_03/HelloWorld` is a useful starting point. Restore its NuGet packages.
3. For Unity examples, open the chapter's project directory with its recorded Unity editor version. Use the book's chapter instructions for scene selection and UWP export, then build and deploy the exported project through Visual Studio.
4. Choose the device or emulator appropriate to the example. Camera, microphone, spatial mapping, and other hardware-dependent features require the corresponding capabilities and hardware support.

Some projects reference temporary signing certificates that are absent from the checkout. If Visual Studio requests one, create your own development test certificate in the project's signing settings. Examples using external vision services or Vuforia also require your own service configuration; their original APIs and deployment tools may no longer be available.

## Related material

- [Programming for Mixed Reality with Windows 10, Unity, Vuforia, and UrhoSharp — Microsoft Press](https://www.microsoftpressstore.com/store/programming-for-mixed-reality-with-windows-10-unity-9781509306879): book details, chapter contents, and companion downloads.

Bundled third-party libraries and assets remain the work of their respective authors.
