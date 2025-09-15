#!/usr/bin/env dotnet

using System.Security.Cryptography;

var d = new Dictionary<long, List<string>>();

foreach (var p in Directory.EnumerateFiles(".", "*", SearchOption.AllDirectories))
{
    var k = new FileInfo(p).Length;
    d.TryAdd(k, []);
    d[k].Add(p.Replace('\\', '/'));
}

var t = new Dictionary<UInt128, string>();

foreach (var v in d.Values)
{
    if (v.Count == 1)
    {
        continue;
    }

    foreach (var p in v)
    {
        using var s = File.OpenRead(p);
        var k = BitConverter.ToUInt128(MD5.HashData(s));
        if (!t.TryAdd(k, p))
        {
            Console.WriteLine($"ln -f \"{t[k]}\" \"{p}\"");
        }
    }
}
