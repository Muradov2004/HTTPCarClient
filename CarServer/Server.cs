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
        int updatingId = 0;
        if (message.Split(' ', ',').Count() > 1)
        {
            command.Value = new() { Id = Convert.ToInt32(message.Split(' ', ',')[1]) };
            switch (message.Split(' ', ',').Count())
            {
                case 3:
                    command.Value.Marka = message.Split(' ', ',')[2];
                    break;
                case 4:
                    command.Value.Marka = message.Split(' ', ',')[2];
                    command.Value.Model = message.Split(' ', ',')[3];
                    break;
                case 5:
                    command.Value.Marka = message.Split(' ', ',')[2];
                    command.Value.Model = message.Split(' ', ',')[3];
                    command.Value.Year = Convert.ToInt32(message.Split(' ', ',')[4]);
                    break;
                case 6:
                    updatingId = Convert.ToInt32(message.Split(' ', ',')[1]);
                    command.Value = new() { Id = Convert.ToInt32(message.Split(' ', ',')[2]) };
                    command.Value.Marka = message.Split(' ', ',')[3];
                    command.Value.Model = message.Split(' ', ',')[4];
                    command.Value.Year = Convert.ToInt32(message.Split(' ', ',')[5]);
                    break;
                default:
                    break;
            }
        }

        Console.WriteLine('\n' + command.HttpMethod.ToUpper() + '\n');
        switch (command.HttpMethod.ToUpper())
        {
            case "HELP":
                bw.Write("\n\n---------------------\nGET -> get all cars\nGET {Id} -> get car by id\nPOST {Id, Marka, Model, Year} -> add car \nPUT Id {Marka, Model, Year} -> update car\nDELETE Id -> delete car by id\n---------------------\nNOTE : write wtihout curly brackets\n---------------------\n");
                break;
            case "GET":
                if (command.Value is null)
                {
                    StringBuilder sendingMessage = new();
                    foreach (var item in cars)
                        sendingMessage.Append(item.ToString() + '\n');
                    bw.Write(sendingMessage.ToString());
                    break;
                }
                bw.Write(cars.FirstOrDefault(c => c.Id == command.Value.Id)!.ToString());
                break;
            case "POST":
                cars.Add(command.Value!);
                bw.Write("Added car!!!");
                break;
            case "PUT":
                cars.FirstOrDefault(c => c.Id == updatingId)!.Id = command.Value!.Id;
                cars.FirstOrDefault(c => c.Id == updatingId)!.Marka = command.Value.Marka;
                cars.FirstOrDefault(c => c.Id == updatingId)!.Model = command.Value.Model;
                cars.FirstOrDefault(c => c.Id == updatingId)!.Year = command.Value.Year;
                bw.Write("Updateded car!!!");
                break;
            case "DELETE":
                cars.Remove(cars.FirstOrDefault(c => c.Id == command.Value!.Id)!);
                bw.Write("Removed car!!!");
                break;
            default:
                bw.Write("Command not found!");
                break;
        }

    }
}



