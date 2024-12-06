using System.Drawing;
using OpenGlSharp.Utils;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace OpenGlSharp.Models;

public partial class DemoWindow;

// window
partial class DemoWindow
{
    protected static IWindow WindowObj = null!;

    protected GL Gl { get; private set; } = null!;

    protected VertextArrrayObject<float, uint> Vao { get; set; } = null!;

    protected BufferObject<float> Vbo { get; set; } = null!;

    protected BufferObject<uint> Ebo { get; set; } = null!;

    protected Shader Shader { get; set; } = null!;

    public Texture Texture { get; set; } = null!;

    protected FpsUtil Fps { get; set; } = new();

    // todo texture

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public DemoWindow(
        string title = "demo",
        int width = 600,
        int height = 600)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = title;

        WindowObj = Window.Create(options);

        WindowObj.Load += Load;
        WindowObj.Render += Render;
        WindowObj.Resize += Resize;

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(100);
                WindowObj.Title = $"{title} ({Fps.Fps:F2})";
            }
        });
    }

    public void Run()
    {
        WindowObj.Run();
        WindowObj.Dispose();
    }
}

// load
partial class DemoWindow
{
    public virtual unsafe void Load()
    {
        Gl = GL.GetApi(WindowObj);

        Gl.ClearColor(Color.Gray);
    }
}

// render
partial class DemoWindow
{
    public virtual unsafe void Render(double v)
    {
        Gl.Clear(ClearBufferMask.ColorBufferBit);
        Fps.Frame();
    }
}

// Resize
partial class DemoWindow
{
    public virtual unsafe void Resize(Vector2D<int> size)
    {
        Gl.Viewport(size);
    }
}

// others
partial class DemoWindow
{
    public void SetShader(string vertext, string fragment)
        => Shader = new Shader(Gl, vertext, fragment);

    public void SetVbo(Span<float> data)
        => Vbo = new BufferObject<float>(Gl, data, BufferTargetARB.ArrayBuffer);

    public void SetEbo(Span<uint> data)
        => Ebo = new BufferObject<uint>(Gl, data, BufferTargetARB.ElementArrayBuffer);

    public void SetVao()
        => Vao = new VertextArrrayObject<float, uint>(Gl, Vbo, Ebo);
}