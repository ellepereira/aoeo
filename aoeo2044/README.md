#Project Penelope - AoEO2044 Editor Build

Downloads AoEO's build 2044. Preconfigured with Editor access and xlive bypass.

## Installation

* Run AoEO2144.exe
* It will download a file called "Launcher.exe" from MS server.
* You will need to edit your windows HOSTS file:
  * Usually, this file is at `C:\Windows\System32\drivers\etc`
  * Easiest way to be able to edit it is to search "Notepad" on your start menu, right click notepad and "Run as Administrator". Once Notepad opens, do File->Open and go to the directory above. Switch the file type filter to "All Files" and open `HOSTS`.
* Add a new line to HOSTS:
```
10.0.0.3     www.ageofempiresonline.com
```
* Download [Mongoose](https://www.cesanta.com/products/binary) free edition and place it on the same directory as `AoEO2144.exe`.
* Now start the `Launcher.exe` file you downloaded earlier. You will eventually get in a loop of launchers.
* Close all the launchers and go to the `patchTemp` directory that should have been created.
* Run `AoEOnline.exe` -> THIS WILL DOWNLOAD THE GAME.
* Once the download is finished then copy the contents of the `files` directory (in this zip) onto the `root` of your new Age of Empires Online directory (you know, where you just downloaded it to).
   * That means: xlive.dll should be on the same folder as Spartan.exe, game.cfg should be inside "startup" and the XML file inside of "data".


** Start the game.**
Click editor from the main menu. Make up your own playground and then click playtest on the editor. Please share some of your creations and have fun.

** DO NOT RUN AOEONLINE.EXE OR OR LAUNCHER.EXE ** there's nothing to update and my xlive.dll will allow you to start the game without the launcher.

## LICENSE
if it breaks your computer I both:
* Don't care.
* You can't sue me because this is a warning that it may break your computer. Run at your own risk etc etc.