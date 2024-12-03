using OpenGlSharp.Extensions;
using OpenGlSharp.Helper;
using OpenGlSharp.Models;
using Serilog;
using Silk.NET.OpenGL;
using StbImageSharp;
using Texture = OpenGlSharp.Models.Texture;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
    .MinimumLevel.Verbose()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] " +
                                     "[{Namespace} | {Method}] " +
                                     "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var window = new ShaderWindow();
window.Run();

public class ShaderWindow : DemoWindow
{
    // xyz rgb st
    private static readonly float[] _vertext =
    [
        0.5f, -0.5f, 0.0f,     1.0f, 0.0f, 0.0f,     0.0f,0.0f,
        -0.5f, -0.5f, 0.0f,    0.0f, 1.0f, 0.0f,     1.0f,0.0f,
        0.0f,  0.5f, 0.0f,     0.0f, 0.0f, 1.0f,     0.5f,1.0f
    ];

    private static readonly uint[] Index = [0, 1, 2];

    private static readonly string _vertexShader =
        """
        #version 330 core
        
        layout(location=0) in vec3 aPos;
        layout(location=2) in vec2 aSt;

        out vec2 st;

        uniform float time;
        
        void main(){
           	gl_Position = vec4(aPos,1.0);
        
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
        SetShader(_vertexShader, _fragmentShader);

        SetVbo(_vertext);
        SetEbo(Index);

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 3, 3);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 8, 2, 6);

        Texture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));
        Texture.SetParams();
    }

    public override unsafe void Load()
    {
        base.Load();

        LoadValue();
    }

    private DateTime t = DateTime.Now;

    private int T = 4;

    public override unsafe void Render(double v)
    {
        base.Render(v);

        Shader.Use();
        Shader?.Uniform1("sampler", 0);

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 3, GLEnum.UnsignedInt, (void*)0);
    }
}