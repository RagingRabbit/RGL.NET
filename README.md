# RGL.NET
The RGL game utility library ported to C#.

## How to install
The RGL.NET library consists of only a few source files which are almost completely self-contained.
Copy the files you want into your source directory to use them.
Some source files require others to work:

Display.cs depends on Native.cs
OpenGL.cs depends on Native.cs
2D.cs depends on OpenGL.cs

This library uses GLFW 3.2.1 for window management.
To be able to use Display.cs correctly, please download the GLFW binaries from their site:
http://www.glfw.org/
Then rename the GLFW dll to "glfw3.dll" and place it into the application directory.

This should be everything you need to get this library to work.
Please note that it is still in the experimental phase as well as incomplete and not ready for public use.
