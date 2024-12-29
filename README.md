This compiles under Visual Studio 2010 - 2012, .NET 3.5. Should compile fine using "clean solution", "rebuild solution".

This requires the VLC plugins version 3.0.10 - (as of 2024) 3.0.21 32 bit (x86). Unless you change the output CPU configuration
to x64, you'll throw an invalid operation exception when trying to load/access the VLC dlls.
