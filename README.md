# GitWrap
[![pipeline status](https://api.travis-ci.org/ardevd/gitwrap.svg?branch=master)](https://travis-ci.org/ardevd/gitwrap)

GitWrap is a Windows Wrapper for the Linux git executable.

Windows Subsystem for Linux is pretty awesome, but managing two separate git environment is not, especially if you use SSH keys. So, here is a little workaround that lets you call git from the WSL environment through a Windows executable.

GitWrap is experimental and can't be considered complete or reliable. It has been tested with Android Studio and seems to work pretty well when used with a compatible system.

NOTE: Please read through the list of known issues.

## Requirements
- Windows Subsystem for Linux must be installed.
- Git should be installed in the Linux environment (WSL).
- Supports x64 variant of Windows only.
- To avoid most known issues, make sure your Windows 10 installation is fully up to date

## Installation
Download the latest GitWrap release and place the executable anywhere you want. GitWrap is completely portable. Then configure your IDE's git executable path and make it point to GitWrap.exe. It should now be using git from your Linux Subsystem on Windows 10.

It should work with other IDE's that allow you to specify the path to the git executable but I've only tested and verified functionality with Android Studio.

To manually verify that GitWrap is working, open up a command prompt and navigate to the directory containing GitWrap.exe

```
GitWrap.exe --version
git version 1.9.1
```

If you get the expected response from the --version command you should be good to go.

## Usage

If you are running WSL with a non-default wslpath you can update GitWrap to use your specified path.

`GitWrap.exe --setWslPath /root/mnt`

You can verify the setting with `--getWslPath`


## Known Issues
- Using GitWrap manually as a command line tool is a bit cumbersome. Commiting files is pretty much impossible since GitWrap cant interface with external editors for commit messages etc. You should use GitWrap with IDE's and other applications that integrate with git or interact with git directly through WSL.
- Due to a [bug in WSL](https://github.com/Microsoft/WSL/issues/2592) affecting Windows 10 Fall Creators Update, GitWrap would not work when used with certain IDE's such as IDEA (including Android Studio). This has been resolved in Windows 10 build 17025 and any fully updated Windows 10 installation today should not encounter this issue.
- With the initial version of Windows Subsystem for Linux it was impossible to pipe output from a Linux command directly to a Windows application. Gitwrap worked around this by piping output to a temporary file and reading the output from there. Because of this, Windows 10 Creators Update is required for GitWrap version 0.2.0.0 and newer. If you're still on an older version of Windows 10 with WSL you can use GitWrap version 0.1.

If you encounter any other issues, bugs or limitations please report them!
