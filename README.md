# CefEOBrowser

[![Build status](https://ci.appveyor.com/api/projects/status/1qcyyjtf64ohk6mu?svg=true)](https://ci.appveyor.com/project/RadarNyan/cefeobrowser)
#### Cef based Browser for ElectronicObserver
This is a drop-in replacement EOBrowser.exe for [ElectronicObserver](https://github.com/andanteyk/ElectronicObserver)  
This project is now part of [my fork of ElectronicObserver](https://github.com/RadarNyan/ElectronicObserver)

#### Introduction
Since August 2018, KanColle enters 2nd Sequence, which moved from Flash to HTML5, dropping support for Internet Explorer. So I restarted my once abandoned project to make it possible for ElectronicObserver to utilize Chromium as its browser.

This project now has Japanese / Chinese UI, to match the language of ElectronicObserver using it. If you want to contribute more UI language, please check "For Developers" section down below.

This project contains some code copied from [ElectronicObserver](https://github.com/andanteyk/ElectronicObserver), credits goes to everyone contributed to that project :)

#### License
This Project is MIT Licensed

#### Libraries used
* [CefSharp](https://github.com/cefsharp/CefSharp) Chromium Embedded Framework - [BSD](https://opensource.org/licenses/BSD-3-Clause)
* [DynamicJson](http://dynamicjson.codeplex.com) Dynamic Json Structure - [MS-PL](https://opensource.org/licenses/MS-PL)

#### How to build
1. Clone this repository, make sure you're on master branch
2. Load it with Visual Studio 2017, earlier versions not tested
3. Restore nuget packages (Cef.WinForms)
4. Build

#### How to use
1. Copy files (EOBrowser.exe~) and folder (CefEOBrowser) from output folder to ElectronicObserver directory, replace original EOBrowser.exe file.
2. Run ElectronicObserver.exe and CefEOBrowser should be loaded.

#### Experimental features
* Disable GPU (since commit [a362c7f](https://github.com/RadarNyan/CefEOBrowser/commit/a362c7f95a619deb3f87c6cb8711e379dfbeb26f))  
Create file ```nogpu``` (with any or no file extension) under ```CefEOBrowser``` folder.  
Please note disabling GPU __may reslut in performance drop and visual artifacts__, don't do this unless you absolutely need to.
* Bookmarks (since commit [89cafe3](https://github.com/RadarNyan/CefEOBrowser/commit/89cafe36937eff08dac8c844071d24cee7d5c48f))  
Currently there's no gui for editing bookmarks, you can manually edit ```bookmarks.json``` under ```CefEOBrowser``` folder.  
This is an standard json format file encoded in UTF-8, here's an example for all the supported properties:
```json
[{
  "......"
},{
  "title": "Webpage Title, shown in menu and on popup window title",
  "url": "https://url.to.your/website/",
  
  "(optional) width": "Window width when opened, default value below",
  "width": 1200,
  
  "(optional) height": "Window height when opened, default value below",
  "height": 700,
  
  "(optional) font": "Standard Font, the website may or may not use it, default value below",
  "font": "Meiryo"
},{
  "......"
}]
```

#### For Developers
This project contains a "submodule" branch which can be used as a submodule for ElectronicObserver forks. Check my fork's commit [46da847](https://github.com/RadarNyan/ElectronicObserver/commit/46da847db6f5148a134247e47d58ee4b9679b5b8) and [e65c552](https://github.com/RadarNyan/ElectronicObserver/commit/e65c55266057b7c39e4e6d2753deb4f43e025402) for example.
