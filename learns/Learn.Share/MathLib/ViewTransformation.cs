//// ReSharper disable InvalidXmlDocComment


//namespace OpenGlSharp.Helper;

//public static class ViewTransformation
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
//    internal static float[] CreateCameraMatrix(float[] position, float[] lookat, float[] top)
//    {
//        lookat = lookat.ToUnitVector();
//        top = top.ToUnitVector();
//        var axisX = lookat.Cross3(top).ToUnitVector();

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

//        var t = rotate.T();

//        return t.DotProduct(origin).T();
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="position">相机的位置</param>
//    /// <param name="target">看的地方</param>
//    /// <param name="top">相机的上方</param>
//    /// <returns></returns>
//    public static float[] LookAt(float[] position, float[] target, float[] top)
//    {
//        var f = target.Minus(position).ToUnitVector();
//        var s = f.Cross3(top).ToUnitVector();
//        var u = s.Cross3(f);

//        var px = s.Dot3(position);
//        var py = u.Dot3(position);
//        var pz = f.Dot3(position);

//        float[] result = [s[0], s[1], s[2], 0, u[0], u[1], u[2], 0, -f[0], -f[1], -f[2], 0, -px, -py, -pz, 1];
//        return result;
//    }
//}