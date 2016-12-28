# GitWrap
Windows Wrapper for Linux git executable.

Windows Subsystem for Linux is pretty awesome, but managing two separate git environment is not, especially if you use SSH keys. So, here is a little workaround that lets you call git from the WSL environment through a Windows executable.

GitWrap is currently a Proof Of Concept solution and is in no way complete or reliable. It has been tested with Android Studio and seems to work pretty well when set up correctly.

## Requirements
- Windows Subsystem for Linux must be installed. 
- Git should be installed in the Linux environment.
- Supports x64 variant of Windows only. 

## Installation
Download the latest GitWrap release, extract and save the executable anywhere you want. Then configure your IDE's git executable path and make it point to GitWrap.exe. It should now be using git from your Linux Sub System on Windows 10.

It should work with other IDE's but I've only tested and verified functionality with Android Studio.

