> [!IMPORTANT]
> This repository is no longer maintained. The project has been moved [to this repository](https://github.com/GuillermoHazebrouck/VGL).
> The new project only contains the calculation units to be launched in .NET under any OS, so there is no graphical interface.

## Welcome to OpenVOGEL aeromechanics (in .NET)
OpenVOGEL is a project providing .NET computer programs useful to study some characteristics of mechanical systems subjected to aerodynamic loads. OpenVOGEL programs can be used, for instance, to compute approximated airloads over complex configurations of slender and thick surfaces (wings and fuselages), to compute some important aeroelastic characteristics of wings and to analyze the dynamic response of an airplane to a gust.

The project focuses on three diciplines of aeromechanics:

* Steady state aerodynamics of wing-body configurations
* Static and dynamic aeroelasticity of wings
* Rigid-body dynamics of wing configurations

Note that optimization problems are out of scope (although any of our programs can be used as data mining tool for that purpose).

The project provides three different interfaces:

* An integrated graphical interface providing interactive control on data (Windows).
* A plain text console aplication privinding batch modules and post processing scripts (Windows/Linux).
* Standalone .NET libraries (that can be easly linked by any other .NET project in any .NET language) exposing all of the project resources (Windows/Linux).

If you are looking for documentation, then visit our wikibook at https://en.wikibooks.org/wiki/Open_VOGEL.
If you have questions about the software, check first the FAQ at www.openvogel.org.

In name of our small community, welcome, and I hope to see you around!

Guillermo

### How to use our programs

#### Tucan
The simplest way to start working with our programs is through the Tucan interface. This program provides all of the tools necessary to create the geometric models and to load other essencial data. It can also run the three different calculation modules (Steady, Aeroelastic and Free flight) and automatically load the results. Tucan is now only being ported to Windows (Linux support can be added in the future) and it installs by just double clicking the "Setup" file. Windows natively hosts all of the necessary .NET dependencies, so no extra installation is required. 

Note that Tucan uses OpenGL version 1.4 (legacy) and not all graphical cards continue to support this.

#### Console
The alternative to Tucan is using the Console (supported by Windows and Linux). Models can eventually be described in an XML file manually or by an external script without Tucan (this is more laborious, but it lets you automate the process for an specific purpose). The XML file is then loaded in the Console using a console script file.
The advantage of using the Console instead of Tucan is that it lest you run a sequence of simulations at once for a range of variables. By doing this you can obtain, for instance, the polar curves of an airplane without having to run the steady states one by one.
For some batch functions, the Console also outputs some Scilab scripts that provide insight in critical characteristics of a configuration.

Console runs directly on windows by double clikcking the executable. Under Linux you must first install the Mono runtime and then prefix the executable call by "mono".

#### .NET libraries
Finally, all of our .NET libraries (shared by Tucan and Console) are available for free and can be linked to any .NET project. This gives you direct access to our parametric models and calculation algorithms. For advanced users with knowledge in .NET and numerical aerodynamics, this is probably the best way to go.
If using Visual Studio, you can load our project libraries along with your project and compile all together, or you can simply link one of our compiled dll's. The second process ir more simple, but it wont let you adapt the source code.
The OpenVOGEL libraries have been split so as to avoid linking unnecessary objects. If you only need our calculation core, you don't need to link the parametric models nor the graphical stuff.
