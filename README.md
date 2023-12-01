# AlpacaDriverDemo

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

* .Net 6-8

## Resources

* ConformU - https://github.com/ASCOMInitiative/ConformU/releases