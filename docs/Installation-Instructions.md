# Virtual Morse 2023

## Prerequisites

### Software

- [Git](https://git-scm.com/) (optional)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs) → Community Edition
- [Arduino IDE](https://www.arduino.cc/en/software)

### Hardware

- [Windows 11](https://www.microsoft.com/en-us/windows/windows-11) Laptop or Desktop (w/ administrator privileges)
- [Arduino Uno Rev3](https://store.arduino.cc/products/arduino-uno-rev3)

## Building from Source

### Software

1. Open Visual Studio IDE.
1. `File` > `Clone a repository` > https://github.com/andrewsng/assistive-technology.git – (optional: learn more about [version control systems](https://www.git-scm.com/book/en/v2/Getting-Started-About-Version-Control))
1. Enter the Project Solution file `VirtualMorse.sln` if it is not already opened.
1. Ensure the required files (`AddressBook.csv`, & `test.txt`) have been created locally inside the text documents directory.
1. Press ▶️ **Start** to build and run the project.

#### Required Packages
- CsvHelper, 30.0.1
- DotNetEnv, 2.5.0
- MailKit, 4.0.0

See also: [Install and manage packages in Visual Studio using the NuGet Package Manager](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio)

### Hardware

1. Confirm wiring with Arduino board schematic or [Tinkercad](https://www.tinkercad.com/) file.
1. Checkout most recent version of the `.ino` from the [GitHub repository](https://github.com/andrewsng/assistive-technology) `main` branch.
1. Plug device into USB port of the desired computer.
1. `File` > `Open...` the relevant `.ino` file from file system.
1. Code flash to `Verify` first and then `Upload` to physical board.

#### Connection Debugging
- `Tools` > `Board` > `Arduino Uno`
- `Port` > `Serial Ports` > ???

#### Setting Arduino COM Port
1. Plug in Arduino to _any_ USB port ⁉️ 
1. Open Device Manager
1. Open section Ports (COM & LPT) > Arduino Uno
1. _Right Click_ > `Properties` > `Port Settings` > `Advanced` > `COM Port Number` > **COM4**
1. If the COM port is already in use, you will need to manually change the value within VirtualMorse source code.

## Compiled Executable File

### Software

_Coming soon!_

### Hardware

Same instructions as **Building from Source**.