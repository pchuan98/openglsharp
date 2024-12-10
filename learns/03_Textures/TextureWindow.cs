using Learn.Share;
using Learn.Share.Models;
using Silk.NET.OpenGL;
using Texture = Learn.Share.Models.Texture;

namespace _03_Textures;

public class TextureWindow : DemoWindow1
{
    // set vbo and bannerTexture
    private static readonly float[] _vertext =
    [
        0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, 0.0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
    ];

    // set vbo and tilingTexture
    private static readonly float[] _tilingVertext =
    [
         0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  10.0f, 10.0f,
         0.5f, -0.5f, 0.0f,  0.5f, 1.0f, 0.0f,  10.0f, 0.0f,
        -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
        -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 10.0f,
    ];

    private static readonly uint[] Index = [0, 1, 2, 0, 3, 2];

    private static readonly string _vertexShader =
        """
        #version 330 core
        
        layout(location=0) in vec3 aPos;
        layout(location=2) in vec2 aSt;

        out vec2 st;

        uniform float time;
        
        void main(){
           	gl_Position = vec4(aPos,1.0);
        
            vec2 offset = vec2(mod(time,10), 0);
            st = aSt - offset;
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

        var defaultTexture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));

        var tilingTexture = Texture.LoadFromFile(Gl,
            Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"),
            () =>
            {
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);

                // algorithm
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
            });

        var bannerTexture = Texture.LoadFromFile(Gl,
            Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"),
            () =>
            {
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);

                // algorithm
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Nearest);
            });

        Texture = bannerTexture;

        Texture.Bind();
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

        Shader.Use();
        Shader?.Uniform1("time", (float)((DateTime.Now - start).TotalSeconds));
        Shader?.Uniform1("sampler", 0);
        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);
    }
}