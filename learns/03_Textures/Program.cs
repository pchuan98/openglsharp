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

var window = new MipmapTextureWindow();
window.Run();

public class TextureWindow : DemoWindow
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

public class MixTextureWindow : DemoWindow
{
    private Texture texture1, texture2, texture3;

    private static readonly float[] _vertext =
    [
        0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, 0.0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
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
        
            st = aSt;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          in vec2 st;

          out vec4 FragColor;
          
          uniform sampler2D sampler1; // smile
          uniform sampler2D sampler2; // back
          uniform sampler2D sampler3; // random

          void main(){
              vec4 t1 = texture(sampler1, st);
              vec4 t2 = texture(sampler2, st);
              vec4 t3 = texture(sampler3, st);

              vec4 value = mix(t1, t2, 1- t3.x);

              FragColor = vec4(value.rgb, 1.0f);
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

        texture1 = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));
        texture2 = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "container.jpg"));
        texture3 = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "terrain0.jpg"));

        texture1.Bind(TextureUnit.Texture0);
        texture2.Bind(TextureUnit.Texture1);
        texture3.Bind(TextureUnit.Texture2);
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
        Shader?.Uniform1("sampler1", 0);
        Shader?.Uniform1("sampler2", 1);
        Shader?.Uniform1("sampler3", 2);


        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);
    }
}

public class MipmapTextureWindow : DemoWindow
{
    private static readonly float[] _vertext =
    [
        0.5f,  0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
        0.5f, -0.5f, 0.0f,  0.5f, 1.0f, 0.0f,  1.0f, 0.0f,
       -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
       -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
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
           	gl_Position = vec4(aPos * (1f / time),1.0);
        
            st = aSt;
        }
        """;

    private static readonly string _fragmentShader
        = """
          #version 330 core
          in vec2 st;

          out vec4 FragColor;
          
          uniform sampler2D sampler1; // smile
          uniform sampler2D sampler2; // back
          uniform sampler2D sampler3; // random

          void main(){
              vec4 t1 = texture(sampler1, st);
              vec4 t2 = texture(sampler2, st);
              vec4 t3 = texture(sampler3, st);

              vec4 value = mix(t1, t2, 1- t3.x);

              FragColor = vec4(value.rgb, 1.0f);
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

        Texture = Texture.LoadFromFile(Gl, Path.Join(FileHelper.FindFolder("imgs"), "container.jpg"));
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
        Shader?.Uniform1("sampler1", 0);

        Vao.Bind();

        Gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, (void*)0);
    }
}
