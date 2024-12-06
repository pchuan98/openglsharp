using BenchmarkDotNet.Running;
using OpenGlSharp.Benchmark.Others;

var summary = BenchmarkRunner.Run<ArrayMethodBenchmark>();