Xamarin.iOS reference updater
=============================

Starting with Xamarin.iOS (previously called MonoTouch) 6.0 several platform
assemblies started to be signed (monotouch.dll, MonoTouch.Dialog-1.dll and
OpenTK-1.0.dll). This is a problem if you upgrade to Xamarin.iOS 6.0 and
you're using third-party assemblies compiled with previous versions of
MonoTouch. In this case the  third-party assemblies would reference a non-
signed version of the signed assemblies, and your project will not compile
anymore.

This program will rewrite these third-party assemblies to reference the signed
platform versions instead.

This is a command line program, so after compiling the project, you need to
open a terminal window, navigate to the project directory and execute:

    mono iOSRefUpdater.exe /path/to/third/party/assembly.dll

 and the third-party assembly will be converted in-place.

