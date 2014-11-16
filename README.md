Sublight.Core
=============

.NET PCL (Portable Class Library) library from Sublight (www.sublight.me). Library currently targets following platforms: .NET Framework 4.5, Windows 8, Windows Phone 8.1, Xamarin Android and Xamarin iOS

With this library you will be able to add following functionalities to your app:
* automatic movie / TV series title detection for given filename (with IMDb id if possible)
* subtitle search
* subtitle download
* rate subtitles and tag them as synchronized / unsynchronized
* download poster images

## Goals:
* easy to use
* lightweight
* asynchronous operation
* highly portable to many platforms and devices (PCs, tablets, phones, set-top boxes)

## Change Log:

2014-11-16
* GetImdbDetailsByHashOrFileName implementation

2014-11-15
* initial commit
* added async methods for Sublight hash calculation (Sublight.Core.HashUtility class, CalculateHashAsync methods)
* added prototype version of REST API method for Login / Logout
