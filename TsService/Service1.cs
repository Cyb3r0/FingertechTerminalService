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

        utilsNitgen utils;
        public Service1()
        {

            InitializeComponent();

            utils = new utilsNitgen();

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

            TcpListener server = null;
            try
            {
                
                Int32 port = 13000;
               

                IPAddress ip = IPAddress.Parse(File.ReadAllText(@"C:\\Windows\\fingertechts.ini"));

                server = new TcpListener(ip, port);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                server.Start();

               
                Byte[] bytes = new Byte[15000];
                String data = null;
               
                while (true)
                {

                    String digital = null;
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");                   

                    data = null;                  
                    NetworkStream stream = client.GetStream();                 
                    int i = stream.Read(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    
                    switch (data)
                    {
                        case "0":
                            digital = utils.Enroll();
                            break;
                        case "1":
                            digital = utils.Capturar();                          
                            break;
                           
                    }
                    //Converter para array de byte
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(digital);
                    //envia resposta de volta
                    stream.Write(msg, 0, msg.Length);
                    client.Close();
                }
            }
            catch (SocketException e)
            {
              
            }
            finally
            {
               
                server.Stop();
            }


          
        }

     
    }
    
    
}
