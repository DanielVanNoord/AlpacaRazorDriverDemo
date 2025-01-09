# AlpacaDriverDemo

This is a basic Alpaca Driver Template built using .Net with a Blazor UI. It uses the same API Library that the OmniSim does.

This is a work in progress. The eventual deliverables are a git template and a .Net new template. For now this works well enough to build drivers but there may be breaking changes.

This requires .Net 8+ on a Linux, Windows or macOS system to build.

## Getting Started

Fill out the constants in program.cs with your details and default ports.

Add driver code to the Drivers folder.
Each class must inherit the corresponding ASCOM Interface.
It is recommended that you keep your device code in a separate dll.

After adding the driver code with the Interface tell the ASCOM Alpaca System to load it. This is done in Program.CS with the ASCOM.Alpaca.DeviceManager. The demo safety monitor is used in Program.cs. This section is marked with //ToDo for you.

Add the device specific setup UI to the corresponding page in the Pages/Devices folder.

Add any general settings to the Setup.razor page.

Test your driver with ConformU.

## Requirements

* .Net 8+

## Resources

* ConformU - https://github.com/ASCOMInitiative/ConformU/releases