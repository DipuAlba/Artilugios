// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using SeDipuAlba.Artilugios.Benchmarks;

Console.WriteLine("Hello, World!");

//var summary1 = BenchmarkRunner.Run<MyRemoveSymbolsBenchmarks>();
var summary2 = BenchmarkRunner.Run<MyCryptoBenchmarks>();