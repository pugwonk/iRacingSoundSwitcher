# iRacingSoundSwitcher
Switch iRacing sound devices from the command line - useful if you want to play in VR and watch replays on your desktop.

To use:
* Download iRacingSound.exe from the Releases page
* Go into iRacing and set your main audio output and voice audio output to go to your VR headset
* run `iRacingSound vr` 
* The program will create a batch file called `vr.bat` which sets the sound appropriately
* Go into iRacing and set your main audio output and voice audio output to go to your desktop computer
* run `iRacingSound desktop` 
* The program will create a batch file called `desktop.bat` which sets the sound appropriately

Some notes:
* If you move iRacingSound.exe to a different location you'll have to recreate the batch files
* You can do this while the iRacing UI is running, but not while iRacing itself is running
