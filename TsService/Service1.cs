using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {

            InitializeComponent();
            Server();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        private void Server()
        {
            utilsNitgen utils = new utilsNitgen();
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                //IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(address => address.AddressFamily == AddressFamily.InterNetwork).First();
                //IPAddress localAddr = IPAddress.Parse(Dns.GetHostEntry(Dns.GetHostName()));

                IPAddress ip = IPAddress.Parse(File.ReadAllText(@"C:\\Windows\\fingertechts.ini"));

                server = new TcpListener(ip, port);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[10000];
                String data = null;

                // Enter the listening loop.
                while (true)
                {               
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    

                    String digital = utils.Enroll();
                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;
                    //ta
                  
                        // converte data bytes para String ASCII.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, 1);
                        Console.WriteLine("Received: {0}", data);

                      

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(digital);

                        // envia resposta
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    

                    
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
               
                server.Stop();
            }


          
        }
    
    }
}
