# VoltageSystem PC App

Desktop application (C# / Avalonia) that receives voltage measurements from a CH32V003 microcontroller over a serial port, validates them, and stores them in SQLite.

- Firmware (frame protocol, drivers, logic analyzer captures): [VoltageSystem Firmware](https://github.com/AlexBh1304/VoltageSystem-Firmware)
- Peripheral drivers library: [ch32v003-peripheral-drivers](https://github.com/AlexBh1304/ch32v003-peripheral-drivers)

![App demo](docs/demo.gif)

## Features

- Select a serial port and connect to the device (9600 baud)
- Live voltage display
- Frame validation: checksum, field count, frame type, sample count and value range
- Lost-frame counter based on the frame sequence number
- Runs of 50 measurements, saved to SQLite together with the selected source
- Data grid with the stored measurements
- Export of all stored data to CSV

## Frame format

The app parses the ASCII frames sent by the firmware:

```
V,<SEQ>,<SUM>,<N>*<CS>
```

Example: `V,12958,33152,64*4B`

A frame is accepted only if:

1. It contains `*` followed by exactly two checksum characters
2. The 8-bit sum of all characters before `*` matches the checksum
3. It has four fields, the type is `V` and `N` is 64
4. `SEQ` and `SUM` are valid numbers, and `SUM` is between 0 and 65472 (64 x 1023)

Otherwise the error is shown in the UI (for example `Incorrect Checksum` or `Format error`).

The voltage is computed from the raw ADC sum:

```
voltage = (SUM / N) * 3.3 / 1023
```

`SEQ` is tracked with 16-bit wraparound, and any gap is added to the lost-frame counter.

## Project structure

```
Hardware/   Serial port driver
Services/   FrameParser, DatabaseService
Models/     Frame, Measurement, CurrentUser
```

## Build and run

Requires the .NET SDK.

```
dotnet run
```

## Data

Measurements are stored in a local SQLite database (`database.db`), table `RunData`, with the columns `RunId`, `SourceId`, `Voltage` and `TimeStamp`. The export button writes the full table to `RunDataInfo.csv`.

## Next steps

Flag or discard runs that contain lost frames, live plot of the measurements, configurable baud rate.
