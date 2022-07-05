# PMFileGenerator
Emby plugin "Potty Mouth" EDL and adjusted SRT file generator.
```
PMFileGenerator <Full path to srt file> <Full path to muted words file>
```

EDL and muted SRT files created in the same directory as the SRT file.

The new SRT file contains only the lines where the audio will be muted, with the muted word replaced with asterisks.

So for example if this is in the original srt:

```45
00:02:44,631 --> 00:02:48,166
Even with all this extra <muted word>, it's
like a fancy car with a crappy engine.
```

The edl line contains:
```
163.46	167.09	1
```

And the new srt file (extension changed to .muted.srt) contains:
```
3
00:02:44,631 --> 00:02:48,166
Even with all this extra ****, it's
like a fancy car with a crappy engine.
```

The index changed because this new srt fle only contains the lines that were muted.

So if you use the new srt, the audio will be muted but you can at least see what they were saying minus the words you do not want to see.

The source is written for NET6, so even though the released binary is for Windows, binaries can be built from the source for other operating systems.

My muted words file is included in the repository.

SRT class from https://github.com/iivmok/srtlib.net.  Copyright (c) 2014 Iivari Mokelainen
