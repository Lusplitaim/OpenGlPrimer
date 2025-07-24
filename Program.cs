using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using GLBuffer = Silk.NET.OpenGL.Buffer;

namespace OpenGlPrimer
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            var glfw = Glfw.GetApi();
            if (glfw is null)
            {
                Console.WriteLine("Error initializing GLFW instance.");
                return;
            }

            var window = InitWindow(glfw);

            while (!glfw.WindowShouldClose(window))
            {
                glfw.PollEvents();
                glfw.SwapBuffers(window);
            }

            glfw.DestroyWindow(window);
            glfw.Terminate();
        }

        static unsafe WindowHandle* InitWindow(Glfw glfw)
        {
            glfw.Init();
            glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
            glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

            var window = glfw.CreateWindow(640, 480, "Title", null, null);
            if (window is null)
            {
                Console.WriteLine("Error creating window.");
                glfw.Terminate();
                return null;
            }

            glfw.MakeContextCurrent(window);

            return window;
        }
    }
}
