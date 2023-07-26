using System.Net;
using System.Net.Sockets;

TcpClient client = new();
client.Connect(IPAddress.Loopback, 27001);
Console.WriteLine("[Client connected to server]");

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

var command = string.Empty;
var answer = string.Empty;

while (true)
{
    command = Console.ReadLine();
    bw.Write(command);
    answer = br.ReadString();
    Console.WriteLine(answer);
    Console.Clear();
}