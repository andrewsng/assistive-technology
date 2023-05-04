Welcome to the [Virtual Morse 2023](https://github.com/andrewsng/assistive-technology/blob/main/README.md) wiki! 

This serves as a reference guide for the Spring 2023 senior capstone project at the [University of Alaska Fairbanks](https://www.cs.uaf.edu/).

## Table of Contents

* [Email](https://github.com/andrewsng/assistive-technology/wiki/Email)
* [Installation Instructions](https://github.com/andrewsng/assistive-technology/wiki/Installation-Instructions)
* [Printing](https://github.com/andrewsng/assistive-technology/wiki/Printing)
* [System Architecture](https://github.com/andrewsng/assistive-technology/wiki/System-Architecture)
* [System Description](https://github.com/andrewsng/assistive-technology/wiki/System-Description)
* [Unaddressed Notes and Questions](https://github.com/andrewsng/assistive-technology/wiki/Unaddressed-Notes-and-Questions)

#### Project Description
- To clone a local copy of the wiki without any source code: `git clone https://github.com/andrewsng/assistive-technology.wiki.git`

###### Latest Version
The above links are available on the GitHub-hosted [wiki](https://github.com/andrewsng/assistive-technology/wiki), and may not be the most recent copy if pulling from an offline version or forked version of the repository.

## File Structure Tree

```
.
├── docs
│   ├── Josh_Arduino_Board_final.svg
│   ├── TESTING.md
│   └── virtual-morse-2023-architecture.svg
├── .gitignore
├── .env.sample
├── LICENSE
├── README.md
└── src
    ├── Arduino_INO_Files
    │   └── josh_board
    │       └── josh_board.ino
    └── VirtualMorse
        ├── App.config
        ├── Function.cs
        ├── Input
        │   ├── ArduinoComms.cs
        │   ├── FunctionKeyInput.cs
        │   ├── InputSource.cs
        │   ├── Switch.cs
        │   └── SwitchInputEventArgs.cs
        ├── packages.config
        ├── Printer.cs
        ├── Program.cs
        ├── Properties
        │   └── AssemblyInfo.cs
        ├── Speech.cs
        ├── States
        │   ├── CommandState.cs
        │   ├── ConfirmationState.cs
        │   ├── PunctuationState.cs
        │   ├── State.cs
        │   └── TypingState.cs
        ├── VirtualMorse.csproj
        ├── VirtualMorse.sln
        └── WritingContext.cs

8 directories, 28 files
```

## License Information
- This project source code and respective documentation is available under the [MIT License](https://github.com/andrewsng/assistive-technology/blob/main/LICENSE).
- For an archived copy containing the original version of Virtual Morse before the Spring 2023 update, please visit: https://github.com/jackdlarue/virtual-morse

