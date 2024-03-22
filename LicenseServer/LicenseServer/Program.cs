using Rhino.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LicenseServer
{
    public class Program
    {
        static LicenseType licenseType;
        static int limit;
        static Dictionary<string, string> connectedClients;

        static int Main(string[] args)
        {
            connectedClients = new Dictionary<string, string>();
            PopulateMachineID();
            CheckLicenseFile();
            return 0;
        }

        private static void PopulateMachineID()
        {
            string _macID = LicenseServer.GetMacAddress();
            Console.Write("Machine ID: " + _macID + Environment.NewLine);
        }

        private static void CheckLicenseFile()
        {
            if (System.IO.File.Exists("license_float.xml"))
            {
                licenseType = LicenseType.Personal; //Node-based
                if (CheckServerLicense())
                    StartServer();
                else
                    Console.ReadLine();
            }
            else
            {
                Console.WriteLine("License Not Found. Contact with Machine ID");
                licenseType = LicenseType.None;
                Console.ReadLine();
            }
        }

        private static bool CheckServerLicense()
        {
            LicenseType _licenseType;
            string _name, _email;
            int _limit;

            bool _validLicense = LicenseServer.CheckLicenseStatus(out _licenseType, out _name, out _email, out _limit);

            if (_validLicense)
            {
                limit = _limit;
                Console.WriteLine("Server License: Name: " + _name + " Email: " + _email + " Limit: " + _limit.ToString());
                return true;
            }
            else
            {
                Console.WriteLine("Invalid License.");
                return false;
            }

        }

        public static void StartServer()
        {
            IPAddress ipAddress = GetLocalIPAddress();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11001);
            Console.WriteLine("Server IP Address: " + ipAddress);

            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Floating License Server started...");
                while (true)
                {
                    Socket handler = listener.Accept();
                    // Each client connection is processed in a new thread.
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(handler);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void HandleClient(object clientSocket)
        {
            Socket handler = (Socket)clientSocket;
            string data = string.Empty;
            byte[] bytes = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    if (bytesRec == 0)
                    {
                        // Connection closed by the client
                        break;
                    }

                    string _dataReceived = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    string[] _dataReceivedArray = _dataReceived.Split(',');

                    if (_dataReceivedArray.Length == 2)
                    {
                        // Handle check-out logic
                        string _clientID = _dataReceivedArray[0];
                        string _managerIP = _dataReceivedArray[1];

                        Console.WriteLine(_clientID);
                        Console.WriteLine(_managerIP);

                        if (connectedClients.Count < limit)
                        {
                            connectedClients.Add(_clientID, handler.RemoteEndPoint.ToString()); // Store endpoint address as string
                        }
                        else
                        {
                            string _poolConsumedMessage = "LicensesLimitReached";
                            byte[] __poolConsumedMessageByteArray = Encoding.ASCII.GetBytes(_poolConsumedMessage);
                            handler.Send(__poolConsumedMessageByteArray);
                        }
                    }
                    else if (_dataReceivedArray.Length == 1)
                    {
                        // Handle check-in logic
                        string _clientID = _dataReceivedArray[0];
                        connectedClients.Remove(_clientID);
                    }

                    data += _dataReceived;
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    // Connection reset or aborted by the client
                    Console.WriteLine("Client disconnected: " + handler.RemoteEndPoint.ToString());
                    Console.WriteLine(connectedClients.Count);
                    // Remove the disconnected client from the list
                    string clientToRemove = connectedClients.FirstOrDefault(x => x.Value == handler.RemoteEndPoint.ToString()).Key;
                    if (!string.IsNullOrEmpty(clientToRemove))
                    {
                        connectedClients.Remove(clientToRemove);
                        Console.WriteLine(connectedClients.Count);
                    }
                }
                else
                {
                    Console.WriteLine("SocketException: {0}", ex);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
            finally
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }




        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }

    
}
