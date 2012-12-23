# DrNuDownloader

A console application for downloading episodes from DR NU (http://www.dr.dk/tv/).

## Prerequisites

1. Download RMPTDump from http://rtmpdump.mplayerhq.hu/.
2. Make sure the folder containing RTMPDump is listed in your PATH environment variable.

## Usage

### List episodes

    DrNuDownloader.Console.exe /l http://www.dr.dk/tv/program/matador

Lists all episodes of Matador.

    http://www.dr.dk/TV/se/matador/matador-18-24
    http://www.dr.dk/TV/se/matador/matador-17-24
    http://www.dr.dk/TV/se/matador/matador-16-24
    ...

### Download episode

    DrNuDownloader.Console.exe /d http://www.dr.dk/tv/se/matador/matador-18-24

Will download episode 18 of the series Matador.

### Download all episodes

    DrNuDownloader.Console.exe /da http://www.dr.dk/tv/program/matador

Will download all episodes of Matador.
