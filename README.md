# Odyssey Engine
This repository contains the source code for:

#### Odyssey.Renderer
3D engine
#### Odyssey.Epos
Entity Component System Framework
#### Odyssey.Renderer2D
User Interface Library _(in OdysseyUI.sln)_
#### Odyssey.Daedalus
Shader Generator tool _(in OdysseyTools.sln)_

#### Installation notes
In order to take advantage of the supplied _Oddysey.proj_ build script you will need to change the SharpDXSdkBinDir at the top of the file so that it points to the _Bin_ folder of your SharpDX SDK. Use the supplied _net45_ (for Desktop projects) or _Win8Debug_ (for WinRT) configurations to build the project.
#### Samples
* MiniUI _(showcases the main UI controls)_
* DataBinding _(an implementation of a MVVM system using the UI library)_




