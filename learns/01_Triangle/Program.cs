using OpenGlSharp.Extensions;
using OpenGlSharp.Models;
using Serilog;
using Silk.NET.OpenGL;
using Shader = OpenGlSharp.Models.Shader;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
    .MinimumLevel.Verbose()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] " +
                                     "[{Namespace} | {Method}] " +
                                     "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();



var window = new TriangleWindow();

window.Run();

public class TriangleWindow : DemoWindow
{
    private static readonly float[] _firstTriangle =
    [
        -0.9f, -0.5f, 0.0f,  // left 
        -0.0f, -0.5f, 0.0f,  // right
        -0.45f, 0.5f, 0.0f // top 
    ];
    private static readonly float[] _secondTriangle =
    [
        0.0f, -0.5f, 0.0f,  // left
        0.9f, -0.5f, 0.0f,  // right
        0.45f, 0.5f, 0.0f   // top 
    ];

    private static readonly string _vertexShader =
        """
        #version 330 core
        layout(location=0) in vec3 pos;
        void main(){
           	gl_Position = vec4(pos.x ,pos.y ,pos.z,1.0);
        }
        """;

    private static readonly string _fragmentShader1
        = """
          #version 330 core
          out vec4 FragColor;
          void main(){
              FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
          }
          """;
    private static readonly string _fragmentShader2 =
        """
          #version 330 core
          out vec4 FragColor;
          void main(){
              FragColor = vec4(1.0f, 1.0f, 0.2f, 1.0f);
          }
          """;

    private Shader shader1, shader2;
    private BufferObject<float> vbo1, vbo2;
    private VertextArrrayObject<float> vao1, vao2;

    public void LoadValue()
    {
        shader1 = new Shader(Gl, _vertexShader, _fragmentShader1);
        shader2 = new Shader(Gl, _vertexShader, _fragmentShader2);

        vbo1 = new BufferObject<float>(Gl, _firstTriangle, BufferTargetARB.ArrayBuffer);
        vao1 = new VertextArrrayObject<float>(Gl, vbo1);
        vao1.AddVertexAttributePointer(VertexAttribPointerType.Float, 3, 3, 0);

        vbo2 = new BufferObject<float>(Gl, _secondTriangle, BufferTargetARB.ArrayBuffer);
        vao2 = new VertextArrrayObject<float>(Gl, vbo2);
        vao2.AddVertexAttributePointer(VertexAttribPointerType.Float, 3, 3, 0);
    }

    public override unsafe void Load()
    {
        base.Load();

        LoadValue();
    }

    public override unsafe void Render(double v)
    {
        base.Render(v);

        shader1.Use();
        vao1.Bind();
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);

        shader2.Use();
        vao2.Bind();
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}