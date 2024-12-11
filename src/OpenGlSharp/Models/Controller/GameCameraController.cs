using OpenGlSharp.Extensions.Math;

namespace OpenGlSharp.Models;

public class GameCameraController(Camera camera)
    : CameraController(camera)
{
    /// <summary>
    /// 将相机看作原点的平移操作
    /// </summary>
    /// <param name="offset"></param>
    public override void Translate(Vector3 offset)
    {
        // 前后移动
        var moveZ = offset.Z * CameraObj.Front;

        // 左右移动
        var moveX = offset.X * Vector3.Normalize(Vector3.Cross(CameraObj.Front, CameraObj.Up));

        CameraObj.Position += (moveZ + moveX);
    }

    public override void Reset()
    {
        CameraObj.Position = new Vector3(0, 0, 0);

        RotateByEulerAbsolute(-90, 0, 0);
    }

    public override void RotateByEuler(float yaw, float pitch, float roll)
    {
        yaw += Yaw;
        pitch += Pitch;
        roll += Roll;

        RotateByEulerAbsolute(yaw, pitch, roll);
    }

    public override void RotateByEulerAbsolute(float yaw, float pitch, float roll)
    {
        //yaw = float.Clamp(yaw, -179, 179); // (-180,180]
        pitch = float.Clamp(pitch, -89, 89); // [-90,90]
        //roll = float.Clamp(roll, -179, 179); // (-180,180]

        var pr = pitch.AsRadian();
        var yr = yaw.AsRadian();

        var x = float.Cos(yr) * float.Cos(pr);
        var y = float.Sin(pr);
        var z = float.Sin(yr) * float.Cos(pr);

        CameraObj.Front = Vector3.Normalize(new Vector3(x, y, z));

        Yaw = yaw;
        Pitch = pitch;
        Roll = roll;
    }
}

