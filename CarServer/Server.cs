using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;


var text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "CarList.json"));
List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(text)!;



var ip = IPAddress.Loopback;
var port = 27001;
TcpListener listener = new(ip, port);

listener.Start();
Console.WriteLine("Sever started [{0}:{1}]", ip, port);

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);

    while (true)
    {
        var message = br.ReadString();
        Command command = new() { HttpMethod = message.Split(' ')[0], };
        if (message.Split(' ').Count() > 1)
            command.Value = new() { Id = Convert.ToInt32(message.Split(' ')[1]) };


        switch (command.HttpMethod.ToUpper())
        {
            case "HELP":
                bw.Write("\n\nGET -> get all cars\nGET {Id} -> get car by id\nPOST {Id, Marka, Model, Year} -> add car \nPUT Id {Marka, Model, Year} -> update car\nDELETE Id -> delete car by id");
                break;
            case "GET":
                StringBuilder sendingMessage = new();
                foreach (var item in cars)
                    sendingMessage.Append(item.ToString() + '\n');
                bw.Write(sendingMessage.ToString());
                break;
            case "POST":

            case "PUT":
            case "DELETE":
            default:
                break;
        }

    }
}



