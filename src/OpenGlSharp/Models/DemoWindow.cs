using System.Drawing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace OpenGlSharp.Models;

public partial class DemoWindow;

// window
partial class DemoWindow
{
    public static IWindow WindowObj = null!;

    public GL Gl { get; private set; } = null!;

    public VertextArrrayObject<float, uint> Vao { get; set; } = null!;

    public BufferObject<float> Vbo { get; set; } = null!;

    public BufferObject<uint> Ebo { get; set; } = null!;

    public Shader Shader { get; set; } = null!;

    public Texture Texture { get; set; } = null!;

    public IInputContext Input { get; set; }

    protected FpsTimer Fps { get; set; } = new();

    public float Time { get; private set; }

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

    public void Stop() => WindowObj.Close();
}

// load
partial class DemoWindow
{
    public event Action<IWindow> OnLoad;

    public virtual unsafe void Load()
    {
        Gl = GL.GetApi(WindowObj);

        Gl.ClearColor(Color.White);
        Gl.Enable(EnableCap.DepthTest);

        Input = WindowObj.CreateInput();
        _keyboard = Input.Keyboards.FirstOrDefault();

        if (_keyboard is null) throw new Exception();

        OnLoad?.Invoke(WindowObj);

        _keyboard.KeyDown += (_, key, _) =>
        {
            _key = key;
            Interlocked.Increment(ref _keyCount);
        };
        _keyboard.KeyUp += (_, _, _) => Interlocked.Decrement(ref _keyCount);
    }
}

// render
partial class DemoWindow
{
    public event Action<IWindow>? OnRender;

    public event Action<IKeyboard, Key?>? OnKeyPressed;

    private IKeyboard? _keyboard;

    private Key? _key;

    private volatile int _keyCount = 0;

    private readonly DateTime _timeStart = DateTime.Now;

    public virtual unsafe void Render(double v)
    {
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        Fps.Frame();

        Time = (float)(DateTime.Now - _timeStart).TotalSeconds;

        OnKeyPressed?.Invoke(_keyboard!, _keyCount == 0 ? null : _key);
        OnRender?.Invoke(WindowObj);
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