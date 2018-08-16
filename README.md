# CefEOBrowser
Cef (Chromium Embedded Framework) based Browser for ElectronicObserver (https://github.com/andanteyk/ElectronicObserver)

---

This project is intended to create a drop-in replacement EOBrowser.exe for ElectronicObserver.

As _Kantai Collection_ (web game) updating to 2nd season, it's moving from Flash to HTML5. Currently ElectronicObserver is using IE based browser which has stopped feature developing since 2013 with the release of IE11, which could introduce compatibility problem latest web technolegies. So I restarted my once abandoned project to make it possible for ElectronicObserver to utilize Chromium as built-in browser.

I would only continue this project if _Kantai Collection_ (web game) 2nd season is still playable with ElectronicObserver (otherwise what's the point?) Currently there are still features missing and a lot parts uncompleted, I would finish them after the release of 2nd season. ~I might also abandon this project (again) if 2nd season works just fine with IE11 and I'm feeling lazy.~

This project would have a Japanese UI for consistency (since ElectronicObserver is in Japanese). I'm only writing this readme in English because I'm more familiar with it and also I'm not good at writing in Japanese.
---

This project uses CefSharp (https://github.com/cefsharp/CefSharp) for Chromium Embedded Framework.

This project contains code copied directly from ElectronicObserver (https://github.com/andanteyk/ElectronicObserver), credits goes to everyone contributed to that project.

---

To build: clone this repository, load with VS2017, restore nuget packages (Cef.WinForms), and build.

Copy files and folder from output folder to ElectronicObserver directory, replace original EOBrowser.exe file.

Run ElectronicObserver.exe and CefEOBrowser should be loaded.
