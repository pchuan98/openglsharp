using System.Drawing;
using OpenGlSharp;
using OpenGlSharp.Extensions;
using OpenGlSharp.Helper;
using Serilog;
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

    gl.ClearColor(Color.Gray);

    // ebo
    uint[] index = [
        0, 1, 2,
        1, 2, 3];
    ebo = new BufferObject<uint>(gl, index, BufferTargetARB.ElementArrayBuffer);

    // vbo
    float[] vertices = [
        0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f];
    vbo = new BufferObject<float>(gl, vertices, BufferTargetARB.ArrayBuffer);

    shader = new Shader(gl, ShaderHelper.VertexShader, ShaderHelper.FragmentShader);

    abo = new VertextArrrayObject<float, uint>(gl, vbo, ebo);
    abo.AddVertexAttributePointer(VertexAttribPointerType.Float, 6, 3, 0);
    abo.AddVertexAttributePointer(VertexAttribPointerType.Float, 6, 3, 3);
};

var t = DateTime.Now;
var T = 3;

window.Update += v =>
{
    var gl = GL.GetApi(window);

    if ((DateTime.Now - t).Seconds >= T)
        t = DateTime.Now;

    gl.Clear(ClearBufferMask.ColorBufferBit);
    abo?.Bind();
    shader?.Use();

    var scale = (float)(2 * Math.PI * (DateTime.Now - t).TotalMilliseconds / (T * 1000f));
    gl.Uniform1(gl.GetUniformLocation(shader!.Handle, "time"), scale);

    unsafe
    {
        gl.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, null);
    }

    //gl.DrawElements(PrimitiveType.Triangles, 6, GLEnum.UnsignedInt, 0);
    //gl.DrawArrays(GLEnum.Triangles, 0, 3);
};

window.Resize += size =>
{
    var gl = GL.GetApi(window);
    gl.Viewport(size);
};

window.Run();
window.Dispose();