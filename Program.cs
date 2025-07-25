using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

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

            var gl = GL.GetApi(new GlfwContext(glfw, window));
            gl.Viewport(0, 0, 640, 480);
            InitVertices(gl);

            while (!glfw.WindowShouldClose(window))
            {
                glfw.PollEvents();
                gl.Clear((int)GLEnum.ColorBufferBit);
                gl.DrawArrays(GLEnum.Triangles, 0, 3);
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

        static unsafe void InitVertices(GL gl)
        {
            float[] vertices = [
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.0f, 0.5f, 0.0f
            ];

            gl.GenVertexArrays(1, out uint vao);
            gl.BindVertexArray(vao);

            gl.GenBuffers(1, out uint vbo);
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
            gl.BufferData<float>(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), vertices, BufferUsageARB.StaticDraw);

            uint vShader = gl.CreateShader(GLEnum.VertexShader);
            string vShaderSrc = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vShader.vert"));
            gl.ShaderSource(vShader, vShaderSrc);
            gl.CompileShader(vShader);
            if ((GLEnum)gl.GetShader(vShader, GLEnum.CompileStatus) == GLEnum.False)
            {
                gl.GetShaderInfoLog(vShader, 512, null, out string infoLog);
                Console.WriteLine(infoLog);
                return;
            }

            uint fragShader = gl.CreateShader(GLEnum.FragmentShader);
            string fragShaderSrc = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fShader.frag"));
            gl.ShaderSource(fragShader, fragShaderSrc);
            gl.CompileShader(fragShader);
            if ((GLEnum)gl.GetShader(fragShader, GLEnum.CompileStatus) == GLEnum.False)
            {
                gl.GetShaderInfoLog(fragShader, 512, null, out string infoLog);
                Console.WriteLine(infoLog);
                return;
            }

            var shaderProgram = gl.CreateProgram();
            gl.AttachShader(shaderProgram, vShader);
            gl.AttachShader(shaderProgram, fragShader);
            gl.LinkProgram(shaderProgram);
            gl.UseProgram(shaderProgram);
            gl.DeleteShader(vShader);
            gl.DeleteShader(fragShader);
            if ((GLEnum)gl.GetProgram(shaderProgram, GLEnum.LinkStatus) == GLEnum.False)
            {
                Console.WriteLine(gl.GetProgramInfoLog(shaderProgram));
                return;
            }

            gl.VertexAttribPointer(0, 3, GLEnum.Float, normalized: false, stride: 3 * sizeof(float), 0);
            gl.EnableVertexAttribArray(0);
        }
    }
}
