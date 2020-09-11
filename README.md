# WindowsTools
A Visual Studio .Net Core solution holding miscellaneous tools used for windows machines

## Table of contents
* [Technologies](#technologies)
* [File Cleanup](#file-cleanup) - Find large unused files *system cleaning*
	
## Technologies
Project is created with:
* .Net Core 3.1
	
## File Cleanup
Have you ever been about to run out of disk space but can't remember what you don't need on your computer because it's been forever since you've used it??

The File Cleanup is WPF project utilizing MVVM, multi-threading, and an on disk SQLLite database with EF Core as the ORM. It is meant to be a customizable deep scanner with the ability to configure and save scanning profile settings to finding large files that are rarely used to free up system space.

![File Scanner Overview Screen](./images/FileScannerOverview.PNG)
