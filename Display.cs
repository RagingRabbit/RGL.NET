using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

using CSGL;
using static CSGL.CSGL;
using static CSGL.Glfw3;
using static CSGL.OpenGL;

namespace RGL
{
    public class Display
    {

        private static List<Display> displays;

        static Display()
        {
            displays = new List<Display>();

            csglLoadGlfw();
            if (glfwInit() == 0)
            {
                throw new Exception("ERROR: Failed to initialize GLFW");
            }
        }

        private IntPtr handle;

        private Color bg;
        private int interval;

        public Display(uint width = 1280, uint height = 720, string title = "")
        {
            handle = glfwCreateWindow((int)width, (int)height, title, NULL, NULL);
            if (handle == NULL)
            {
                throw new Exception("ERROR: Failed to create window");
            }

            glfwMakeContextCurrent(handle);
            csglLoadGL();

            displays.Add(this);

            background = Color.Black;
            interval = 0;

            glfwSwapInterval(interval);
        }

        ~Display()
        {
            glfwDestroyWindow(handle);
            displays.Remove(this);
            if (displays.Count == 0)
            {
                glfwTerminate();
            }
        }

        public void update()
        {
            glfwSwapInterval(interval);

            glfwPollEvents();
            glfwSwapBuffers(handle);

            glClearColor(bg.R / 255.0f, bg.G / 255.0f, bg.B / 255.0f, bg.A / 255.0f);
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        }

        public Color background
        {
            get { return bg; }
            set { bg = value; }
        }

        public int swapInterval
        {
            get { return interval; }
            set { interval = value; }
        }

        public bool open
        {
            get { return glfwWindowShouldClose(handle) == 0; }
        }

        public string version
        {
            get
            {
                IntPtr stringPtr = glGetString(GL_VERSION);
                string versionString = Marshal.PtrToStringUTF8(stringPtr);
                return versionString;
            }
        }

    }
}
