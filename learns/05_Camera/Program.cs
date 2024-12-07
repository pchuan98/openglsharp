using System.Numerics;
using System.Runtime.CompilerServices;
using OpenGlSharp.Extensions;
using OpenGlSharp.Helper;
using OpenGlSharp.MathLib;
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

var window = new CameraWindow();
window.Run();

public class CameraWindow : DemoWindow
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        0.5f,  0.5f, 0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, 0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -0.5f, -0.5f, 0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -0.5f,  0.5f, 0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
    ];

    private static readonly uint[] Index = [0, 1, 2];

    private static readonly string _transVertexShader =
        """
        #version 330 core

        layout(location=0) in vec3 aPos;
        layout(location=2) in vec2 aSt;

        out vec3 pos;
        out vec2 st;

        uniform float time;
        uniform mat4 transform;

        void main(){
           	gl_Position = transform * vec4(aPos, 1.0);
        
            st = aSt;
            pos = aPos;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          in vec2 st;
          in vec3 pos;

          out vec4 FragColor;
          
          uniform sampler2D sampler;

          void main(){
              FragColor = vec4(pos,1.0);
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

        var model = ModelTransformation.RotateX(-time * 30);


        Shader?.UniformMatrix44("transform", model);

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);

    }
}