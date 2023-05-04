# Unaddressed Notes and Questions

This page lists certain notes and questions for the client that were unable to be addressed, as well as a few thoughts given to what steps could be taken if they were to be addressed.


## Notes

### Executable file should be compiled from source and made to run at the startup of the machine.

This hasn't been fully tested by the team, but Visual Studio offers an option at `Build > Publish Virtual Morse` that walks you through the installation of the built project at some given location on the machine. Once this executable file is installed somewhere, there should be various methods online to have that file be run at Windows startup.

### Some places in the "Current System Info" document say that the email address should be read out letter by letter, but this has not been implemented.

Creating a Prompt that is spoken out letter by letter has been done before with reading out entered letters and punctuation, so the same method could be applied here.

### Hyperlinks in emails are read out in full.

This has partly been alleviated since the reading of emails can be cancelled. Going further with this might involve retrieving the HTML data from MailKit and trying to identify hyperlinks with that.


## Questions

### Should punctuation entered through the 2nd command mode fill the "last entered letter" used when repeatedly hitting Enter?

This seems like a good idea and consistent with the rest of Virtual Morse.

### Should the "last entered letter" used when repeatedly hitting Enter respect capitalization from Shift?

This is probably not necessary.

### Currently, hitting Backspace while there is morse-in-progress will delete a letter from the current word or document, but keep the morse string. Should Backspace instead do nothing when there is morse-in-progress?

It might take some time using the system to see if the current behavior is bothersome, and if changes are needed.