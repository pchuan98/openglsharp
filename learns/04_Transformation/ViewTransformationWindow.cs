using System.Numerics;
using Learn.Share;
using Learn.Share.Models;
using Silk.NET.OpenGL;
using Texture = Learn.Share.Models.Texture;
namespace _04_Transformation;


public class ViewTransformationWindow : DemoWindow1
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    ];

    private static readonly uint[] Index = [0, 1, 2, 0, 3, 2];

    private static readonly string _transVertexShader =
        """
        #version 330 core

        layout(location=0) in vec3 aPos;
        layout(location=1) in vec2 aSt;

        out vec2 st;

        uniform float time;
        
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;

        void main(){
            vec4 pos = projection * view * model * vec4(aPos, 1.0);

           	gl_Position = pos;

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
        SetEbo(Enumerable.Range(0, _vertext.Length).Select(item => (uint)item).ToArray());

        SetVao();
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 3, 0);
        Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 2, 3);

        Texture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"), () =>
        {
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);

            // filter
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Repeat);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Repeat);
        });
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

        var model = MathHelper.CreateRotate([1, 1, 1], 10);

        var view = MathHelper.LookAt([float.Sin(time), 0, float.Cos(time)], [0, 0, 0], [0, 1, 0]);
        //var per = MathHelper.CreatePerspectiveProjection((float)Math.PI / 2f, 1.0f * WindowObj.Size.X / WindowObj.Size.Y, 1f, 1f, 10000f, 1f);
        var projection = Matrix4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, 1, -1);

        //var projection = MathHelper.CreatePerspectiveProjection(-1, 1, -1, 1, 1, -1);
        //var projection = MathHelper.CreateOrghographicProjection(1);

        Shader?.UniformMatrix44("model", model, true);
        Shader?.UniformMatrix44("view", view, true);
        Shader?.UniformMatrix44("projection", projection, false);

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, (uint)_vertext.Length, GLEnum.UnsignedInt, (void*)0);

    }
}