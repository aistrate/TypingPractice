Typing Practice Helper
======================

This is a little app that helps you improve your typing skills, including:

* general speed (assuming you can already touch-type)
* proficiency on specific types of text (e.g., programming language source code)
* speed on specific types of characters (e.g., numbers, symbols)

![Normal text](https://raw.github.com/aistrate/TypingPractice/master/Screenshot01.png "Normal text")

![Source code](https://raw.github.com/aistrate/TypingPractice/master/Screenshot02.png "Source code")

![Special characters](https://raw.github.com/aistrate/TypingPractice/master/Screenshot03.png "Special characters")

It also includes a couple of practice text generators (import from Wikipedia articles, import from source code, generation of random characters). These will allow you to practice on a variety of texts of your own choosing.


Downloads
---------

Zipped executables can be downloaded here:

* [Typist.zip](http://dl.dropbox.com/u/14656944/Typist/Typist.zip) -- all executables, including font installer (7 MB)
* [TypistLite.zip](http://dl.dropbox.com/u/14656944/Typist/TypistLite.zip) -- all executables, not including font installer (120 kB)

Main executables:

* Typist.exe -- practicing of typing skills
* FontInstall.msi -- install 27 free fixed-width fonts, to be used by Typist.exe (Note: Typist.exe will work fine with standard fonts found on any Windows machine; installing extra fonts will just offer more options.)

Executables that create practice texts (to be used with Typist.exe):

* TypingTextCreator.exe -- import articles from Wikipedia
* SourceCodeTextCreator.exe -- clean up and format source code files, and split them into chunks small enough for practice; by default it assumes C-like syntax, but can also process other programming languages
* CharTrainingCreator.exe -- generate blocks of random characters, for practice directed to a particular character group (e.g., digits only)


Keyboard Shortcuts
------------------

* Ctrl-P: Start/Stop typing practice
* Ctrl-O: Open 'Import File' dialog
* Ctrl-X: Open 'Settings' dialog
* Ctrl-F: Open 'Font Properties' dialog

* Ctrl-N: Use next font
* Ctrl-Shift-N: Use previous font
* Ctrl-+: Increase size of current font (by 0.25 point)
* Ctrl--: Decrease size of current font (by 0.25 point)
* Ctrl-T: Set current font and font size as default (persistent)

Some of these actions can also be accessed from the context menu (right-click on text area of the window). The context menu also contains a list of predefined font & size pairs.


Fonts
-----

A good font to use is 'Anonymous Pro, 27.25 point'. (It is not set as default, because this font family might not be installed on your machine, unless you used FontInstall.msi).

To set it (after running FontInstall.msi):

1. Go to: context (right-click) menu > 'Predefined Fonts' > 'Anonymous Pro (24 point)'
2. Press Ctrl-+ to make the font size larger (or Ctrl-- to make it smaller), until you reach 27.25 point (visible on the status bar at the bottom)
