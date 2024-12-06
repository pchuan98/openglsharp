using System.Numerics;
using System.Runtime.CompilerServices;
using OpenGlSharp.Extensions;
using OpenGlSharp.Helper;
using OpenGlSharp.Models;
using OpenGlSharp.Utils;
using Serilog;
using Silk.NET.OpenGL;
using Texture = OpenGlSharp.Models.Texture;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
    .MinimumLevel.Verbose()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] " +
                                     "[{Namespace} | {Method}] " +
                                     "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var window = new ViewTransformationWindow();
window.Run();

public class ModelTransformationWindow : DemoWindow
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        0.5f,  0.5f, 0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, 0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -0.5f, -0.5f, 0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -0.5f,  0.5f, 0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
    ];

    private static readonly uint[] Index = [0, 1, 2, 0, 3, 2];

    private static readonly string _transVertexShader =
        """
        #version 330 core

        layout(location=0) in vec3 aPos;
        layout(location=2) in vec2 aSt;

        out vec2 st;

        uniform float time;
        uniform mat4 transform;

        void main(){
            vec4 pos = vec4(aPos, 1.0);
        
           	gl_Position = transform * pos;
        
            st = aSt;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          in vec2 st;

          out vec4 FragColor;
          
          uniform sampler2D sampler;

          void main(){
              FragColor = texture(sampler,st);
          }
          """;


    public unsafe void LoadValue()
    {
        SetShader(_transVertexShader, _fragmentShader);

        SetVbo(_vertext);
        SetEbo(Index);

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 3);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 2, 6);

        Texture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));
    }

    public override unsafe void Load()
    {
        base.Load();

        LoadValue();
    }

    DateTime start = DateTime.Now;

    public override unsafe void Render(double v)
    {
        base.Render(v);
        var time = (float)((DateTime.Now - start).TotalSeconds);

        Shader.Use();

        Shader?.Uniform1("time", time);

        Shader?.Uniform1("sampler", 0);

        var right = MathHelper.CreateTranslation(0.5f);
        var left = MathHelper.CreateTranslation(-0.5f);
        var rotate = MathHelper.CreateRotateZ(time * 100);
        var scale = MathHelper.CreateScaleMain((float)Math.Sin(time), (float)Math.Sin(time));

        Shader?.UniformMatrix44("transform", rotate.Dot(scale));

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);

    }
}

public class ViewTransformationWindow : DemoWindow
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        1.5f,  1.5f, 0.0f,    1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        1.5f, -1.5f, 0.0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -1.5f, -1.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -3f,  1.5f, 0.0f,   0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
    ];

    private static readonly uint[] Index = [0, 1, 2, 0, 3, 2];

    private static readonly string _transVertexShader =
        """
        #version 330 core

        layout(location=0) in vec3 aPos;
        layout(location=2) in vec2 aSt;

        out vec2 st;

        uniform float time;
        uniform mat4 transform;

        void main(){
            vec4 pos = vec4(aPos, 1.0);
        
           	gl_Position = transform * pos;
        
            st = aSt;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          in vec2 st;

          out vec4 FragColor;
          
          uniform sampler2D sampler;

          void main(){
              FragColor = texture(sampler,st);
          }
          """;


    public unsafe void LoadValue()
    {
        SetShader(_transVertexShader, _fragmentShader);

        SetVbo(_vertext);
        SetEbo(Index);

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 3);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 2, 6);

        Texture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));
    }

    public override unsafe void Load()
    {
        base.Load();

        LoadValue();
    }

    DateTime start = DateTime.Now;

    public override unsafe void Render(double v)
    {
        base.Render(v);
        var time = (float)((DateTime.Now - start).TotalSeconds);

        Shader.Use();

        Shader?.Uniform1("time", time);

        Shader?.Uniform1("sampler", 0);

        var model = MathHelper.CreateRotateY(time * 30);
        var camera = MathHelper.CreateCameraMatrix([float.Sin(time), float.Cos(time), 0], [0, 0, 1], [0, 1, 0]);
        var per = MathHelper.CreatePerspectiveProjection((float)Math.PI / 2f, 1.0f * WindowObj.Size.X / WindowObj.Size.Y, 1f, 1f, 10000f, 1f);
        Shader?.UniformMatrix44("transform", per);

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);

    }
}