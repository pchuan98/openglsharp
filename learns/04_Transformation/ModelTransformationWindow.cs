using Learn.Share;
using Learn.Share.Models;
using Silk.NET.OpenGL;
using Texture = Learn.Share.Models.Texture;

namespace _04_Transformation;

public class ModelTransformationWindow : DemoWindow1
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