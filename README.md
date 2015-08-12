# eve-log-watcher

App is watching log file for changes, parsing solar system names from new entries and showing them in dotlan-style map if they are close enouch to player current location.

Player current location is automatically updated after each jump by watching 'Local' chatlog. Or can be manually selected (for gathering intel about surrounding non-local system)

http://kos.cva-eve.org/ integrated check:
- USAGE: select local player names, hit Ctrl+C, then shortcut defined in main window (in screenhot Contrl+Shift+F), after a while new dialog will appear with red background in player/corp/alliance that is currently in kos list
- all kos request are cached in memory until app is closed
- change shortcut: click into box on main form, press desired shortcut and then click apply button.
	
Screenhot: (in less than 10 seconds ago there was intel about 'Mamet' solar system)
![Screenhot](https://raw.githubusercontent.com/CzBuCHi/eve-log-watcher/master/screenhot.png)
