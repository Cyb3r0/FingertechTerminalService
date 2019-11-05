using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;




using System.Threading;


namespace TsService
{
    public partial class Service1 : ServiceBase
    {

        TcpListener server;
        Thread thread;
        utilsNitgen utils;
        public Service1()
        {

            InitializeComponent();
            EventLog.WriteEntry("service1", EventLogEntryType.Warning);
            utils = new utilsNitgen();
            //thread = new Thread(Server);
            //thread.Start();
           
            

        }

        protected override void OnStart(string[] args)
        {
            utils = new utilsNitgen();
            EventLog.WriteEntry("Serviço Iniciando", EventLogEntryType.Warning);
             thread = new Thread(Server);
            thread.Start();
            EventLog.WriteEntry("Serviço Iniciado", EventLogEntryType.Warning);

        }

        protected override void OnStop()
        {
            server.Stop();
            thread.Abort();
            

        }

        private void Server()
        {

            
            server = null;
            try
            {
                
                Int32 port = 13000;
               

                IPAddress ip = IPAddress.Parse(File.ReadAllText(@"C:\\Windows\\fingertechts.ini"));
                server = new TcpListener(ip, port);
               
                server.Start();
               
                Byte[] bytes = new Byte[15000];
                String data = null;
               
                while (true)
                {
                    EventLog.WriteEntry("while", EventLogEntryType.Warning);

                    String digital = null;
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");                   

                    data = null;                  
                    NetworkStream stream = client.GetStream();                 
                    int i = stream.Read(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    
                    switch (data)
                    {
                       
                        case "1":
                            try
                            {
                                EventLog.WriteEntry("capturar", EventLogEntryType.Warning);
                                digital = utils.Capturar();
                                break;
                            }
                            catch(Exception e)
                            {
                                                              
                                break;
                            }

                           

                            
                           
                    }
                    //Converter para array de byte
                    EventLog.WriteEntry("passei o case", EventLogEntryType.Warning);
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(digital);
                    //envia resposta de volta
                    stream.Write(msg, 0, msg.Length);
                    client.Close();
                    EventLog.WriteEntry("finalizei whileiado", EventLogEntryType.Warning);
                }
            }
            catch (SocketException e)
            {
                EventLog.WriteEntry("Serviço interrompido "+e, EventLogEntryType.Error);
            }
            finally
            {
               
                server.Stop();
            }


          
        }

     
    }
    
    
}
