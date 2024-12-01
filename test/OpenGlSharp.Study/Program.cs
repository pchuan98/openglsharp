using System.Drawing;
using OpenGlSharp;
using OpenGlSharp.Extensions;
using Serilog;
using OpenGlSharp.LogExtension;
using OpenGlSharp.Models;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Shader = OpenGlSharp.Models.Shader;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithCallerInfo(["OpenGlSharp", "OpenGlSharp.Study"])
    .MinimumLevel.Verbose()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:}] " +
                                     "[{Namespace} | {Method}] " +
                                     "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();


var option = WindowOptions.Default;
option.Size = new Vector2D<int>(600, 600);
option.Title = "demo window";

var window = Window.Create(option);

BufferObject<float>? vbo = null;
BufferObject<uint>? ebo = null;
VertextArrrayObject<float, uint>? abo = null;
Shader? shader = null;

window.Load += () =>
{
    var gl = GL.GetApi(window);

    gl.ClearColor(Color.White);

    // ebo
    uint[] index = [
        0, 1, 3,
        1, 2, 3];
    ebo = new BufferObject<uint>(gl, index, BufferTargetARB.ElementArrayBuffer);

    // vbo
    float[] vertices = [
        0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 1.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
        -0.5f,  0.5f, 0.5f, 0.0f, 0.0f
    ];
    vbo = new BufferObject<float>(gl, vertices, BufferTargetARB.ArrayBuffer);

    abo = new VertextArrrayObject<float, uint>(gl, vbo, ebo);
    abo.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
    abo.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);



    shader = new Shader(
        gl,
        """
        #version 330 core
        layout(location = 0) in vec3 pos;

        void main(){
           	gl_Position = vec4(pos.x,pos.y,pos.z,1.0);
        }
        """,
        """
        #version 330 core
         
        out vec4 color;
         
        void main(){
            color = vec4(1.0f, 0.0f, 0.0f, 1.0f);
        }
        """);
};

window.Update += v =>
{
    var gl = GL.GetApi(window);

    gl.Clear(ClearBufferMask.ColorBufferBit);
    abo?.Bind();
    shader?.Use();
    unsafe
    {
        gl.DrawElements(PrimitiveType.Lines, (uint)2, DrawElementsType.UnsignedInt, null);

    }


    //gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, 0);
    //gl.DrawArrays(GLEnum.Triangles, 0, 3);
};

window.Run();
window.Dispose();