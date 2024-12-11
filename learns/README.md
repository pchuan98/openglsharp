# Learn

该部分主要是opengl的学习成果，其中01到04依赖于共享项目Learn.Share,里面包含手写的推导矩阵

## Todo

- [x] 三角形显示
- [x] shader编辑加载
- [x] texture渲染
- [x] model、view、projection的变换矩阵
- [ ] 齐次坐标系方程，统一整个camera变换

## Note

- [ ] 测试发现自己写的基于 `Span<double>` 的点乘的效率会高于Matrix4x4，要想办法迁移优化一下