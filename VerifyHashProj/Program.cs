using System;
using BCrypt.Net;
using System.IO;

class Program
{
    static void Main()
    {
        string hash = BCrypt.Net.BCrypt.HashPassword("T4k3d4@@!");
        Console.WriteLine("Length: " + hash.Length);
        Console.WriteLine("Hash: " + hash);
        File.WriteAllText("hash_new.txt", hash);
    }
}
