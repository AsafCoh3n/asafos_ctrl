using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
class Program
{
    static void Main(string[] args)
    {

        IPAddress ip = IPAddress.Parse("10.0.0.64");
        int port = 29000;
        TcpClient client = new TcpClient();
        Console.WriteLine("trying to connect to asafos");
        client.Connect(ip, port);
        Console.WriteLine("asafos connected!!\ntype {>?} for help menu");
        NetworkStream ns = client.GetStream();
        Thread thread = new Thread(o => ReceiveData((TcpClient)o));
        client.ReceiveTimeout = 5000000;


        thread.Start(client);
        string s;
        while (!string.IsNullOrEmpty((s = Console.ReadLine())))
        {
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            ns.Write(buffer, 0, buffer.Length);
        }

        client.Client.Shutdown(SocketShutdown.Send);
        thread.Join();


        ns.Close();
        client.Close();
        Console.WriteLine("disconnect from asafos!!");
        Console.ReadKey();
    }

    static int pac_num;

    static void ReceiveData(TcpClient client) {
        NetworkStream ns = client.GetStream();
        byte[] receivedBytes = new byte[1024];
        int byte_count;

        DateTime endRunAt = DateTime.Now.AddSeconds(1);

        while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
        {
			
            char[] input = Encoding.ASCII.GetChars(receivedBytes, 0, byte_count);
            var onlyLetters = new String(input.SkipWhile(p => !Char.IsLetter(p) && !Char.IsDigit(p)).ToArray());
        
			      Console.WriteLine("${0}",input);//raw input without filter


        }
    }
}
