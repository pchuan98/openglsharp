namespace OpenGlSharp.Extensions.Math;

public static class RotateExtension
{

    public static Matrix Rotate(this Matrix matrix, Vector3 axis, float radian)
    {
        float[] u = [axis.X, axis.Y, axis.Z];

        var cosx = MathF.Cos(radian);
        var cosx1 = 1 - cosx;

        var sinx = MathF.Sin(radian);

        var norm = MathF.Sqrt(u[0] * u[0] + u[1] * u[1] + u[2] * u[2]);

        var ux = u[0] / norm;
        var uy = u[1] / norm;
        var uz = u[2] / norm;

        var uxy = ux * uy;
        var uxz = ux * uz;
        var uyz = uy * uz;

        var n11 = cosx + ux * ux * cosx1;
        var n12 = uxy * cosx1 - uz * sinx;
        var n13 = uxz * cosx1 + uy * sinx;

        var n21 = uxy * cosx1 + uz * sinx;
        var n22 = cosx + uy * uy * cosx1;
        var n23 = uyz * cosx1 - ux * sinx;

        var n31 = uxz * cosx1 - uy * sinx;
        var n32 = uyz * cosx1 + ux * sinx;
        var n33 = cosx + uz * uz * cosx1;

        //float[] result = [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];


        return new Matrix(n11, n21, n31, 0, n12, n22, n32, 0, n13, n23, n33, 0, 0, 0, 0, 1);
    }
}