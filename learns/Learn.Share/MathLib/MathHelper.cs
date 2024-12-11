//// ReSharper disable InvalidXmlDocComment

//using System.ComponentModel.DataAnnotations;
//using System.Numerics;
//using MathNet.Numerics.LinearAlgebra;

//namespace OpenGlSharp.Helper;

//public static partial class MathHelper
//{
//    /// <summary>
//    /// 角度转弧度
//    /// </summary>
//    internal const double RADIAN_SCALE = Math.PI / 180.0;

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="data"></param>
//    /// <param name="n"></param>
//    /// <returns></returns>
//    /// <exception cref="NotSupportedException"></exception>
//    [Obsolete("目前没有验证正确性")]
//    public static float[,] To2D(this float[] data, int n) => Matrix<float>.Build.Dense(n, n, data).Transpose().ToArray();

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="data"></param>
//    /// <returns></returns>
//    [Obsolete("目前没有验证正确性")]
//    public static float[] To1D(this float[,] data) => Matrix<float>.Build.DenseOfArray(data).ToRowMajorArray();

//    /// <summary>
//    /// 去除四维矩阵的第四列
//    /// </summary>
//    /// <param name="data"></param>
//    /// <returns></returns>
//    public static float[] To33(this float[] data)
//    {
//        throw new NotSupportedException();
//    }

//    /// <summary>
//    /// 向量减法
//    /// </summary>
//    /// <param name="a"></param>
//    /// <param name="b"></param>
//    /// <returns></returns>
//    public static float[] Minus(this float[] a, float[] b)
//    {
//        if (a.Length != b.Length) throw new ArgumentException();

//        var count = a.Length;
//        var result = new float[count];

//        for (var i = 0; i < count; i++)
//            result[i] = a[i] - b[i];

//        return result;
//    }

//    /// <summary>
//    /// 任意向量变成单位向量
//    /// </summary>
//    /// <param name="vector"></param>
//    /// <returns></returns>
//    public static float[] ToUnitVector(this float[] vector)
//    {
//        var sum = (float)Math.Sqrt(vector.Sum(i => i * i));

//        return vector.Select(item => item / sum).ToArray();
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="array"></param>
//    /// <returns></returns>
//    public static string AsString(this float[] array)
//    {
//        var length = (int)Math.Sqrt(array.Length);
//        var str = "";

//        for (var i = 0; i < length; i++)
//        {
//            for (var j = 0; j < length; j++)
//                str += $"{array[i * length + j]:F4}\t";
//            str += Environment.NewLine;
//        }

//        return str;
//    }

//    public static string AsString(this float[,] array)
//    {
//        var length = (int)Math.Sqrt(array.Length);
//        var str = "";

//        for (var i = 0; i < length; i++)
//        {
//            for (var j = 0; j < length; j++)
//                str += $"{array[i, j]:F4}\t";
//            str += Environment.NewLine;
//        }

//        return str;
//    }
//}

//#region Model Transformation

//// cal
//partial class MathHelper
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="a"></param>
//    /// <param name="b"></param>
//    /// <returns></returns>
//    [Obsolete]
//    public static float[] DotProduct(this float[] a, float[] b)
//    {
//        // 默认列读取数据
//        var ma = Matrix<float>.Build.Dense(4, 4, a).Transpose();
//        var mb = Matrix<float>.Build.Dense(4, 4, b).Transpose();

//        return (ma * mb).ToRowMajorArray();
//    }

//    /// <summary>
//    /// a . b 点乘
//    /// </summary>
//    /// <param name="a"></param>
//    /// <param name="b"></param>
//    /// <returns></returns>
//    public static float[] Dot(this float[] a, float[] b)
//    {
//        var result = new float[16];
//        DotProductBySpan(a.AsSpan(), b.AsSpan(), result.AsSpan());
//        return result;
//    }

//    private static void DotProductBySpan(ReadOnlySpan<float> a, ReadOnlySpan<float> b, Span<float> result)
//    {
//        if (a.Length != 16 || b.Length != 16 || result.Length != 16)
//            throw new ArgumentException("must be a 4 x 4 matrix");

//        for (var row = 0; row < 4; row++)
//        {
//            for (var col = 0; col < 4; col++)
//            {
//                float sum = 0;
//                for (var k = 0; k < 4; k++)
//                {
//                    // a[row,k] * b[k,col]
//                    sum += a[row * 4 + k] * b[k * 4 + col];
//                }
//                result[row * 4 + col] = sum;
//            }
//        }
//    }

//    /// <summary>
//    /// 矩阵转置
//    /// </summary>
//    /// <param name="matrix"></param>
//    /// <returns></returns>
//    public static float[] T(this float[] matrix)
//    {
//        if (matrix.Length != 16)
//            throw new ArgumentException("must be a 4 x 4 matrix");

//        return [matrix[0], matrix[4], matrix[8], matrix[12],
//            matrix[1], matrix[5], matrix[9], matrix[13],
//            matrix[2], matrix[6], matrix[10], matrix[14],
//            matrix[3], matrix[7], matrix[11], matrix[15]];
//    }

//    /// <summary>
//    /// 向量a叉乘b（三维向量）
//    /// </summary>
//    /// <param name="a"></param>
//    /// <param name="b"></param>
//    /// <returns></returns>
//    public static float[] Cross3(this float[] a, float[] b)
//    {
//        var x = a[1] * b[2] - a[2] * b[1];
//        var y = a[2] * b[0] - a[0] * b[2];
//        var z = a[0] * b[1] - a[0] * b[1];

//        return [x, y, z];
//    }
//}

//// translation
//partial class MathHelper
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="tx"></param>
//    /// <param name="ty"></param>
//    /// <param name="tz"></param>
//    /// <returns></returns>
//    public static float[] CreateTranslation(float tx = 0, float ty = 0, float tz = 0)
//        => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];
//}

//// rotate
//// note: 旋转矩阵正交，满足ATA = AAT = I ==> A = A(-1)
//partial class MathHelper
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="theta"></param>
//    /// <returns></returns>
//    public static float[] CreateRotateX(float theta = 0)
//    {
//        var radian = theta * RADIAN_SCALE;

//        var sinx = (float)Math.Sin(radian);
//        var cosx = (float)Math.Cos(radian);

//        return [1, 0, 0, 0, 0, cosx, sinx, 0, 0, -sinx, cosx, 0, 0, 0, 0, 1];
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="theta"></param>
//    /// <returns></returns>
//    public static float[] CreateRotateY(float theta = 0)
//    {
//        var radian = theta * RADIAN_SCALE;

//        var sinx = (float)Math.Sin(radian);
//        var cosx = (float)Math.Cos(radian);

//        return [cosx, 0, -sinx, 0, 0, 1, 0, 0, sinx, 0, cosx, 0, 0, 0, 0, 1];
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="theta"></param>
//    /// <returns></returns>
//    public static float[] CreateRotateZ(float theta = 0)
//    {
//        var radian = theta * RADIAN_SCALE;

//        var sinx = (float)Math.Sin(radian);
//        var cosx = (float)Math.Cos(radian);

//        return [cosx, sinx, 0, 0, -sinx, cosx, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="u"></param>
//    /// <param name="theta"></param>
//    /// <returns></returns>
//    /// <exception cref="ValidationException"></exception>
//    public static float[] CreateRotate(float[] u, float theta = 0)
//    {
//        var radian = theta * RADIAN_SCALE;

//        var cosx = (float)Math.Cos(radian);
//        var cosx1 = 1 - cosx;

//        var sinx = (float)Math.Sin(radian);

//        var norm = (float)Math.Sqrt(u[0] * u[0] + u[1] * u[1] + u[2] * u[2]);

//        var ux = u[0] / norm;
//        var uy = u[1] / norm;
//        var uz = u[2] / norm;

//        var uxy = ux * uy;
//        var uxz = ux * uz;
//        var uyz = uy * uz;

//        var n11 = cosx + ux * ux * cosx1;
//        var n12 = uxy * cosx1 - uz * sinx;
//        var n13 = uxz * cosx1 + uy * sinx;

//        var n21 = uxy * cosx1 + uz * sinx;
//        var n22 = cosx + uy * uy * cosx1;
//        var n23 = uyz * cosx1 - ux * sinx;

//        var n31 = uxz * cosx1 - uy * sinx;
//        var n32 = uyz * cosx1 + ux * sinx;
//        var n33 = cosx + uz * uz * cosx1;

//        return [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];
//    }
//}

//// scale
//partial class MathHelper
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="tx"></param>
//    /// <param name="ty"></param>
//    /// <param name="tz"></param>
//    /// <returns></returns>
//    public static float[] CreateScaleMain(float tx = 1, float ty = 1, float tz = 1)
//        => [tx, 0, 0, 0, 0, ty, 0, 0, 0, 0, tz, 0, 0, 0, 0, 1];

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="u"></param>
//    /// <param name="k"></param>
//    /// <returns></returns>
//    /// <exception cref="ValidationException"></exception>
//    public static float[] CreateScale(float[] u, float k)
//    {
//        var scale = k - 1;

//        var norm = (float)Math.Sqrt(u[0] * u[0] + u[1] * u[1] + u[2] * u[2]);

//        var ux = u[0] / norm;
//        var uy = u[1] / norm;
//        var uz = u[2] / norm;

//        var uxy = ux * uy;
//        var uxz = ux * uz;
//        var uyz = uy * uz;

//        var n11 = 1 + scale * ux * ux;
//        var n12 = scale * uxy;
//        var n13 = scale * uxz;

//        var n21 = scale * uxy;
//        var n22 = 1 + scale * uy * uy;
//        var n23 = scale * uyz;

//        var n31 = scale * uxz;
//        var n32 = scale * uyz;
//        var n33 = 1 + scale * uz * uz;

//        return [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];
//    }
//}

//#endregion

//#region View / Camera Transformation

//partial class MathHelper
//{
//    /// <summary>
//    /// 创建相机（视图）变换矩阵
//    /// 
//    /// 理解方式为逆向移动物体，平移 -> 旋转
//    /// </summary>
//    /// <param name="position"></param>
//    /// <param name="lookat">
//    /// 看的方向
//    /// </param>
//    /// <param name="top">
//    /// 相机的上方
//    /// </param>
//    /// <returns></returns>
//    public static float[] CreateCameraMatrix(float[] position, float[] lookat, float[] top)
//    {
//        lookat = lookat.ToUnitVector();
//        top = top.ToUnitVector();

//        // translate to origin
//        float[] origin = [
//            1, 0, 0, -position[0],
//            0, 1, 0, -position[1],
//            0, 0, 1, -position[2],
//            0, 0, 0, 1];

//        // inverse
//        var lx = lookat[0];
//        var ly = lookat[1];
//        var lz = lookat[2];

//        var tx = top[0];
//        var ty = top[1];
//        var tz = top[2];

//        var axisX = lookat.Cross3(top).ToUnitVector();
//        var ltx = axisX[0];
//        var lty = axisX[1];
//        var ltz = axisX[2];

//        /**
//         * note
//         * * 条件
//         * * 1. 相机朝向 -> l
//         * * 2. 相机上方 -> t
//         *
//         * 首先，可以利用l和t向量的叉乘得到需要的x
//         * 可以反向推导，
//         * 将-z变成l，将y变成t，求取逆矩阵
//         * 由于旋转矩阵是正交矩阵，所以矩阵的逆矩阵是转置
//         *
//         * | 1    0    0    0 |             | lx x tx    tx    -lx    0 |
//         * | 0    1    0    0 |      =>     | ly x ty    ty    -ly    0 |
//         * | 0    0    1    0 |      =>     | lz x tz    tz    -lz    0 |
//         * | 0    0    0    1 |             | 0          0     0      1 |
//         *
//         * 正交的充要条件： 当每一行 or 列 的结果都是单位向量且两两垂直
//         */

//        float[] rotate = [
//            ltx, tx, -lx, 0,
//            lty, ty, -ly, 0,
//            ltz, tz, -lz, 0,
//            0, 0, 0, 1];

//        var rotateT = rotate.T();

//        return rotateT.Dot(origin);
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="position"></param>
//    /// <param name="target"></param>
//    /// <param name="top"></param>
//    /// <returns></returns>
//    public static float[] LookAt(float[] position, float[] target, float[] top)
//        => CreateCameraMatrix(position, target.Minus(position), top);
//}

//#endregion

//#region Projection Transformation

//partial class MathHelper
//{
//    /// <summary>
//    /// 正交投影 (结果变成-1到1)
//    /// </summary>
//    /// <param name="l"></param>
//    /// <param name="r"></param>
//    /// <param name="b"></param>
//    /// <param name="t"></param>
//    /// <param name="f"></param>
//    /// <param name="n"></param>
//    /// <returns></returns>
//    public static float[] CreateOrghographicProjection(
//        float l, float r, float b, float t, float f, float n)
//    {
//        // center cuboid by tranlating
//        var trans = CreateTranslation(-(l + r) / 2, -(t + b) / 2, -(f + n) / 2);

//        // scale into canonical cube
//        var scale = CreateScaleMain(2 / (r - l), 2 / (t - b), 2 / (n - f));

//        return scale.Dot(trans);
//    }

//    /**
// * 关于透视投影
// * 1. 近平面不变
// * 2. 中心点不变
// * 3. 近平面(n)和远平面(f)的z不变，中间的z会有所变化
// * 4. f > n > 0
// *
// * 求解方法是 求解某个矩阵，将frustum（视锥体）压缩成为一个长方体，然后把这个长方体进行正交变换
// *
// * 通过相似三角形的关系，n的值，对任意z，有， (我们所期望的长方体应该是压缩到近平面的比例下)
// * zx or zy = (nx or ny) * nz / zz
// *  ==>
// * | n    0    0    0 |  | x |      | xn |
// * | 0    n    0    0 |  | y |  ==  | yn |
// * | A    B    C    D |  | z |  ==  | ?  |
// * | 0    0    0    z |  | 1 |      | z  |
// * (ABCD)为未知待定系数
// *
// * 加入特值点
// * * 在near上 任一点(i,j,n)变换结果不变
// * * 在far上，任一点(k,l,f)变换结果中，z恒定f
// *
// * Ai + Bj + Cn + D = n*n
// * Ak + Bl + Cf + D = f*f
// *
// * 使用特值法，当nf=0的时候 + ijkl为任意一点 => A = B = 0
// *
// * Cn + D = n*n
// * Cf + D = f*f
// * ==> C = n + f ,  D = -nf
// */

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="l"></param>
//    /// <param name="r"></param>
//    /// <param name="b"></param>
//    /// <param name="t"></param>
//    /// <param name="f"></param>
//    /// <param name="n"></param>
//    /// <returns></returns>
//    public static float[] CreatePerspectiveProjection(
//        float l, float r, float b, float t, float f, float n)
//    {
//        //float[] trans = [
//        //    n, 0, 0, 0,
//        //    0, n, 0, 0,
//        //    0, 0, n + f, -n * f,
//        //    0, 0, 0, n];

//        //var ortho = CreateOrghographicProjection(l, r, b, t, f, n);
//        //var per = ortho.Dot(trans);


//        float[] per = [
//            (2 * n) / (r - l), 0, -(r + l) / (r - l), 0,
//            0, 2 * n / (t - b), -(t + b) / (t - b), 0,
//            0, 0, (n + f) / (n - f), -2 * n * f / (n - f),
//            0, 0, 1, 0];
//        //return per;
//        var matrix = Matrix4x4.CreatePerspectiveFieldOfView(l, r, n, f);

//        return [matrix.M11, matrix.M12, matrix.M13, matrix.M14,
//            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
//            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
//            matrix.M41, matrix.M42, matrix.M43, matrix.M44];
//    }
//}

//#endregion