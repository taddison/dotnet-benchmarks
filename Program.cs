using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
  public static void Main(string[] args) =>
    BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);
}

public class DictionaryBenchmark
{
  private static readonly Dictionary<string, int> _dictionary = new Dictionary<string, int>();
  private const string KEY_STRING = "abcdefghijklmnopqrstuvwxyz";
  private const int TARGET_MAX_LENGTH = 200;

  [GlobalSetup]
  public void Setup()
  {
    foreach (var key in GetKeyValues())
    {
      _dictionary.Add((string)key, 1);
    }
  }

  [Benchmark]
  [ArgumentsSource(nameof(GetKeyValues))]
  public int TryGetValue(string key) => _dictionary.TryGetValue(key, out var value) ? value : default;

//https://benchmarkdotnet.org/articles/features/parameterization.html#sample-introargumentssource
  public IEnumerable<object> GetKeyValues()
  {
    var keyString = KEY_STRING;
    while(keyString.Length < TARGET_MAX_LENGTH)
    {
      keyString += KEY_STRING;
    }

    for (int i = 1; i <= keyString.Length; i += 20)
    {
      yield return keyString.Substring(0, i);
    }
  }
}