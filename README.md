![](https://sites.google.com/site/gahvogel/_/rsrc/1459943467902/config/customLogo.gif?revision=14)

###Welcome!
In this project we work together to bring out a basic computer program useful to solve aerodynamic problems, 100% free and for everyone.
If you have questions about the software, or if you want to share data and/or chat with other users, you can join the community (visit www.openvogel.com).

![](https://sites.google.com/site/gahvogel/_/rsrc/1455897404095/main/Air.png?height=171&width=320)![](https://sites.google.com/site/gahvogel/_/rsrc/1457720308039/main/WindTurbine12.png?height=161&width=200)

If you are willing to give us a hand with the development, stay here, you are at the right place. You are never too late to join this project, as there is an infinite number of things that can be done. The greatest value of any open source project is that everyone can learn something from it, and we all can benefit from the resulting product.

Open VOGEL has been programmed in VB.NET and C#, which are integral part of the open sourced .NET framework. This is a very mature and advanced development platform that makes programming easier than ever, while still providing a very good level of calculation performance (less efficient than C++, but much easier to work with and certainly a lot faster than any implementation in an interpreted language like Python or Matlab).

For the GUI, VOGEL is based in WinForms and SharpGL. If you are familiar with at least one of these programming languages and API's, and you have a good knowledge of aerodynamics, algebra or finite element methods, you can help us with the development. A strong spirit of cooperation is cernainly also required.

If you are new to collaborative open source development, follow the next steps:

- Download and install Visual Studio 2015 Community (or later version) in your computer
- Add the GitHub plugin to VS and sign in.
- Fork this project to a local repository (download the source code into a local folder in your computer).
- Read the quick introduction to the source code.
- Commit to an issue from the list (or submit a new issue).
- Start working on the code.
- When you think the job is done (after you have tested it at least a couple of times and it seems to work), create a new pull request (https://help.github.com/articles/using-pull-requests/).
- Discuss with the group members about possible issues or corrections (code styling, comments, bugs, etc.).
- Solve possible issues.
- Waint until a member with push access merges your changes in the main branch.

####Quick introduction to the source code

The Open VOGEL solution comprises three main projects: the `AeroTools` class library, the `MathTools` class library and the `OpenVOGEL` winforms project.

#####AeroTools
In the `AeroTools` library you will find the core definitions divided in two parts: the visual model and the calculation model.
The `VisualModel` namespace (folder) contains all the objects users intercact with while designing a model and the components of the graphical user interface. This library avoids any method related to the analysis, and it only holds the global information that needs to be provided by the user (geometry, structural properties, polars, etc.). The `CalculationModel` namespace, on the other hand, is a more abstract part of the library that houses the whole calculation core. It is divided in an aerodynamic part, and a structural part. This library deals with the aerodynamic and structural methods, containing a collection of calculation algorithms and precise definitions of the calculation data (vortex rings, lattices, structural elements, etc.).

In OpenVOGEL the GUI and the calculation core are split (they are contained in different classes). When you start the calculation, the design model is converted into a calculation model, and when the calculation finishes, the calculation core is loaded into a container that will represent the results. The calculation core is also kept in memory after calculation (or when reloading results) so that the user can still compute the velocity field and the global airloads.

The design model contains a collection of `Surface` objects. The `Surface` class is a must-inherit class that is common to all design models (lifting surfaces, fuselages, nacelles, ext.), providing the basic common features we need to handle them. These features are mainly: a mesh, a description of the position and orientation, the visual properties (colors and visibility), an overridable method to generate the mesh, and an overridable method to represent the model in an OpenGL container. This means that you can easly introduce a new type of surface by inheriting the `Surface` class and implementing your own meshing procedure. Of course that if you do that, you will also need to create the forms required to edit the data and the conversion algorithms to put the model in the calculation core.

#####MathTools
The `MathTools` library contains important basic definitions that have been created to solve several vector and matrix problems that can be found all over the solution. The most important library here is probably the one under the `EuclideanSpace` namespace, since it contains basic vector algebra (2D and 3D vectors).

Othe important namespaces in this library are `EigenValues` and `Integration`, the former used to solve generalized eigen value problems (Kx = lMx), and the last one used to solve ODEs. Both libraries are used by the aeroelastic solver.

#####The OpenVOGEL winforms project and the GUI
The winforms project is actually quite small, as it only contains the main form and the splash screen. Instead of dropping all GUI controls in the main form, OpenVOGEL keeps the GUI components in specific user controls and forms that are dynamically loaded during runtime. This means that when you open the MainForm in Visual Studio, you will not be able to do much from the designer. If you want to modify the layout or the behavior of some GUI component, then you have to go to that specific form or user control.
Working this way results in a tidy program structure, because you can provide a form or user control with only the information it will handle, which is most of the times sealed in a specific class. Just to give an example, the `WingControl` panel only loads and handles the information contained in a single `LiftingSurface` object, and it may additionally access the polar database.

The main form contains a SharpGL control for the 3D representation of the models and a `MainRibbon` control that is in charge of the topmost administrative job. This last component is part of the `AeroTools` class library and allows users to handle the unique instance to the `ProjectRoot` class, which holds all of the information in the software.

####Some rules
Comments in the code should be written in simple English. For practical reasons, we will not accept code written in other languages.
When publishing documentation about a certain module, try to write it in English as well, or try to produce a translated version. There are many people from all over the world interested in this project, so we have to keep a uniform format in order to understand eachother. Writing in other languages outside the code is, however, not forbidden.


Welcome to our project and thanks for your cooperation!
