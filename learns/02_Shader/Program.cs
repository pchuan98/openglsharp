using OpenGlSharp.Extensions;
using OpenGlSharp.Models;
using Serilog;
using Silk.NET.OpenGL;

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
    private static readonly float[] _vertext =
    [
        0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  // bottom left
        0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f   // top 
    ];

    private static readonly uint[] Index = [0, 1, 2];

    private static readonly string _vertexShader =
        """
        #version 330 core
        
        layout(location=0) in vec3 aPos;
        layout(location=1) in vec3 aColor;
        
        out vec3 color;
        out vec3 position;

        uniform float time;
        
        void main(){
            float offset = cos(time) * 0.5;
        
           	gl_Position = vec4(aPos.x + offset ,aPos.y ,aPos.z,1.0);
            color = aColor * (cos(time) + 1.0) / 2.0;
        
            position = aPos;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          
          in vec3 color;
          in vec3 position;
          
          out vec4 FragColor;
          
          uniform float time;
          
          void main(){
              
              float intensity = (sin(time)+1.0)/2.0;
          
              FragColor = vec4(vec3(intensity) + color ,1f);
          }
          """;


    public void LoadValue()
    {
        SetShader(_vertexShader, _fragmentShader);

        SetVbo(_vertext);
        SetEbo(Index);

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 6, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 6, 3, 3);
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
        Shader?.Uniform1(
            "time",
            (float)(2 * Math.PI * (DateTime.Now - t).TotalMilliseconds / (T * 1000f)));

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 3, GLEnum.UnsignedInt, (void*)0);
    }
}