# PMFileGenerator
Emby plugin "Potty Mouth" EDL and adjusted SRT file generator.

PMFileGenerator <Full path to srt file> <Full path to muted words file>
EDL and muted SRT files created in the same directory as the SRT file.

The new SRT file contains only the lines where the audio will be muted, with the muted word replaced with asterisks.

SRT class from https://github.com/iivmok/srtlib.net.  Copyright (c) 2014 Iivari Mokelainen
