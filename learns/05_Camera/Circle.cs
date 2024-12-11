using System.Numerics;
using OpenGlSharp.Extensions;
using OpenGlSharp.Extensions.Math;
using OpenGlSharp.Helper;
using OpenGlSharp.Models;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Texture = OpenGlSharp.Models.Texture;

namespace _05_Camera;

public static class CircleDemo
{
    public static void Run()
    {
        Serilog.Log.Logger.ConfigLoggerDefault();

        var demo = new DemoWindow();

        var camera = new Vector3();
        var speed = 0.1f;

        var yaw = 0f;

        demo.OnKeyPressed += (kb, k) =>
        {
            if (k is null) return;

            if (k == Key.Escape)
                demo.Stop();

            if (kb.IsKeyPressed(Key.W))
                camera.Z -= speed;
            if (kb.IsKeyPressed(Key.S))
                camera.Z += speed;
            if (kb.IsKeyPressed(Key.A))
                camera.X -= speed;
            if (kb.IsKeyPressed(Key.D))
                camera.X += speed;

        };

        demo.OnLoad += w =>
        {
            demo.SetShader(
                """
        #version 330 core
        layout(location=0) in vec3 aPos;
        layout(location=1) in vec2 aSt;

        out vec3 pos;
        out vec2 st;
        
        uniform float time;
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 proj;
        
        void main(){
            gl_Position = proj * view * model * vec4(aPos, 1.0);

            st = aSt;
            pos = aPos;
        }
        """,
                """
        #version 330 core
        out vec4 FragColor;
        in vec2 st;
        in vec3 pos;
        uniform sampler2D texture1;
        uniform sampler2D texture2;
        void main(){
          FragColor = mix(texture(texture1, st), texture(texture2, st), 0.8);
        }
        """);

            float[] vertext = [
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
                0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
                0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,

                -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
                0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
                0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
                0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
                -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,

                -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
                -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
                -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

                0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
                0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
                0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
                0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
                0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
                0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
                -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

                -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
                0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
                0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
                -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
                -0.5f, 0.5f, -0.5f, 0.0f, 1.0f
                ];

            demo.SetVbo(vertext);
            demo.SetEbo(Enumerable.Range(0, vertext.Length).Select(item => (uint)item).ToArray());

            demo.SetVao();
            demo.Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 3, 0);
            demo.Vao.AddVertexAttributePointer(VertexAttribPointerType.Float, 5, 2, 3);

            var texture1 = Texture.LoadFromFile(demo.Gl, Path.Join(FileHelper.FindFolder("imgs"), "awesomeface.png"));
            var texture2 = Texture.LoadFromFile(demo.Gl, Path.Join(FileHelper.FindFolder("imgs"), "container.jpg"));
            texture1.Bind();
            texture2.Bind(TextureUnit.Texture1);
        };

        float[] offset = [
            0.0f, 0.0f, 0.0f,
            2.0f, 5.0f, -15.0f,
            -1.5f, -2.2f, -2.5f,
            -3.8f, -2.0f, -12.3f,
            2.4f, -0.4f, -3.5f,
            -1.7f, 3.0f, -7.5f,
            1.3f, -2.0f, -2.5f,
            1.5f, 2.0f, -2.5f,
            1.5f, 0.2f, -1.5f,
            -1.3f, 1.0f, -1.5f
            ];

        var proj = Matrix4x4.CreatePerspectiveFieldOfView(
            45f.AsRadian(), 1f, 0.1f, 100f);
        //var proj = Matrix4x4.CreateOrthographicOffCenter(-10f, 10f, -10f, 10f, 10f, -10f);


        demo.OnRender += _ =>
        {
            var model = Matrix4x4.Identity;
            //var view = Matrix4x4.CreateLookTo(camera, new Vector3(0, 0, -1), new Vector3(0, 1, 0));
            var view = Matrix4x4.CreateLookAt(
                new Vector3(10 * float.Sin(demo.Time.AsRadian() * 60),
                0, 10 * float.Cos(demo.Time.AsRadian() * 60)),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0));

            demo.Shader.Use();
            demo.Shader.Uniform1("time", demo.Time);
            demo.Shader.UniformMatrix44("model", model);
            demo.Shader.UniformMatrix44("view", view);
            demo.Shader.UniformMatrix44("proj", proj);

            demo.Shader.Uniform1("texture1", 0);
            demo.Shader.Uniform1("texture2", 1);

            demo.Vbo.Bind();

            for (var i = 0; i < 10; i++)
            {
                var angle = 20.0f * i;
                var x = offset[i * 3];
                var y = offset[i * 3 + 1];
                var z = offset[i * 3 + 2];
                var trans = Matrix4x4.CreateTranslation(x, y, z);
                var rotate = Matrix4x4.Identity.Rotate(new Vector3(1.0f, 0.3f, 0.5f), angle.AsRadian());
                //Matrix44Extension.DotProduct(trans, rotate)
                demo.Shader.UniformMatrix44("model", rotate * trans);
                unsafe
                {
                    demo.Gl.DrawElements(PrimitiveType.Triangles, 36, GLEnum.UnsignedInt, (void*)0);
                }
            }
        };

        demo.Run();

    }
}