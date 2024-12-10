using System.Numerics;
using OpenGlSharp.Extensions;
using OpenGlSharp.Helper;
using OpenGlSharp.MathLib;
using OpenGlSharp.Models;
using Serilog;
using Silk.NET.OpenGL;
using Texture = OpenGlSharp.Models.Texture;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
    .MinimumLevel.Verbose()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] "
            + "[{Namespace} | {Method}] "
            + "{Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

var window = new CameraWindow();
window.Run();

public class CameraWindow : DemoWindow
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        // top
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        0.0f,
        0.5f,
        -0.5f,
        -0.5f,
        1.0f,
        0.0f,
        0.5f,
        0.5f,
        -0.5f,
        1.0f,
        1.0f,
        0.5f,
        0.5f,
        -0.5f,
        1.0f,
        1.0f,
        -0.5f,
        0.5f,
        -0.5f,
        0.0f,
        1.0f,
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        0.0f,
        -0.5f,
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        0.5f,
        -0.5f,
        0.5f,
        1.0f,
        0.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        1.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        1.0f,
        -0.5f,
        0.5f,
        0.5f,
        0.0f,
        1.0f,
        -0.5f,
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        -0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        -0.5f,
        0.5f,
        -0.5f,
        1.0f,
        1.0f,
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        -0.5f,
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        -0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        0.5f,
        0.5f,
        -0.5f,
        1.0f,
        1.0f,
        0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        0.5f,
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        0.5f,
        -0.5f,
        -0.5f,
        1.0f,
        1.0f,
        0.5f,
        -0.5f,
        0.5f,
        1.0f,
        0.0f,
        0.5f,
        -0.5f,
        0.5f,
        1.0f,
        0.0f,
        -0.5f,
        -0.5f,
        0.5f,
        0.0f,
        0.0f,
        -0.5f,
        -0.5f,
        -0.5f,
        0.0f,
        1.0f,
        -0.5f,
        0.5f,
        -0.5f,
        0.0f,
        1.0f,
        0.5f,
        0.5f,
        -0.5f,
        1.0f,
        1.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        0.5f,
        0.5f,
        0.5f,
        1.0f,
        0.0f,
        -0.5f,
        0.5f,
        0.5f,
        0.0f,
        0.0f,
        -0.5f,
        0.5f,
        -0.5f,
        0.0f,
        1.0f,
    ];

    private static readonly string _vertexShader = """
        #version 330 core

        layout(location=0) in vec3 aPos;
        layout(location=1) in vec2 aSt;

        out vec3 pos;
        out vec2 st;

        uniform float time;

        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;

        void main(){
           	gl_Position = projection * view * model * vec4( aPos, 1.0);

            st = aSt;
            pos = aPos;
        }
        """;

    private static readonly string _fragmentShader = """
        #version 330 core
        out vec4 FragColor;

        in vec2 st;
        in vec3 pos;

        uniform sampler2D texture1;
        uniform sampler2D texture2;

        void main(){
          FragColor = mix(texture(texture1, st), texture(texture2, st), 0.8);
        }
        """;

    private readonly float[] _cubePositions =
    [
        0.0f,
        0.0f,
        0.0f,
        2.0f,
        5.0f,
        -1.0f,
        -1.5f,
        -2.2f,
        -2.5f,
        -3.8f,
        -2.0f,
        -12.3f,
        2.4f,
        -0.4f,
        -3.5f,
        -1.7f,
        3.0f,
        -7.5f,
        1.3f,
        -2.0f,
        -2.5f,
        1.5f,
        2.0f,
        -2.5f,
        1.5f,
        0.2f,
        -1.5f,
        -1.3f,
        1.0f,
        -1.5f,
    ];

    private readonly DateTime start = DateTime.Now;

    private Texture texture1,
        texture2;

    public void LoadValue()
    {
        SetShader(_vertexShader, _fragmentShader);

        SetVbo(_vertext);
        SetEbo(Enumerable.Range(0, _vertext.Length).Select(item => (uint)item).ToArray());

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 2, 3);

        texture1 = Texture.LoadFromFile(
            Gl,
            Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png")
        );
        texture2 = Texture.LoadFromFile(
            Gl,
            Path.Join(FileHelper.FindFolder("imgs"), "container.jpg")
        );

        texture1.Bind();
        texture2.Bind(TextureUnit.Texture1);
    }

    public override void Load()
    {
        base.Load();

        LoadValue();
    }

    public override unsafe void Render(double v)
    {
        base.Render(v);
        var time = (float)(DateTime.Now - start).TotalSeconds;

        Shader.Use();

        Shader?.Uniform1("time", time);

        Shader?.Uniform1("texture1", 0);
        Shader?.Uniform1("texture2", 1);

        var value = time;

        var view = ViewTransformation.LookAt(
            [float.Sin(value), 0, float.Cos(value)],
            [0, 0, 0],
            [0, 1, 0]
        );
        //view = Matrix44Extension.One;
        //var projection = ProjectionTransformation.Perspective((float)WindowObj.Size.X / (float)WindowObj.Size.Y);
        var projection = ProjectionTransformation.Orghographic(10f);
        //var projection = ProjectionTransformation.Orghographic(-1, 1, -1, 1, -1, 1);

        Shader?.UniformMatrix44(
            "view",
            Matrix4x4.CreateLookAt(
                new Vector3([float.Sin(value), 0, float.Cos(value)]),
                new Vector3([0, 0, 0]),
                new Vector3([0, 1, 0])
            )
        );
        Shader?.UniformMatrix44(
            "projection",
            Matrix4x4.CreateOrthographicOffCenter(-10, 10, -10, 10, 10, -10)
        );

        Vao.Bind();

        for (var i = 0; i < 2; i++)
        //for (var i = 0; i < 3; i++)
        {
            var angle = 20.0f * i;

            var x = _cubePositions[i * 3];
            var y = _cubePositions[i * 3 + 1];
            var z = _cubePositions[i * 3 + 2];

            var trans = ModelTransformation.Translation(x, y, z);
            //var rotate = ModelTransformation.Rotate([1.0f, 0.3f, 0.5f], angle);
            //Matrix44Extension.DotProduct(trans, rotate)

            Shader?.UniformMatrix44("model", trans);

            Gl.DrawElements(
                PrimitiveType.Triangles,
                (uint)_vertext.Length,
                GLEnum.UnsignedInt,
                (void*)0
            );
        }
    }
}
