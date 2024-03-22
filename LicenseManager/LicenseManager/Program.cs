using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rhino.Licensing;

namespace LicenseManager
{
    public class Program
    {
        static LicenseType licenseType;
        static Socket managerToServerSocket;


        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            PopulateMachineID();
            CheckLicenseFile();
            return 0;
        }

        private static void PopulateMachineID()
        {
            string _macID = LicenseManager.GetMacAddress();
            Console.Write("Machine ID: " + _macID + Environment.NewLine);
        }

        private static void CheckLicenseFile()
        {
            if (System.IO.File.Exists("license.xml"))
            {
                licenseType = LicenseType.Personal; //Node-based
                if (CheckNodeLockedLicense())
                    StartManagerToSlaveServer();
                else
                    Console.ReadLine();
            }
            else if (System.IO.File.Exists("server-ip.txt"))
            {
                licenseType = LicenseType.Floating;
                string serverIp = System.IO.File.ReadAllText("server-ip.txt").Trim();
                Console.WriteLine("Attempting to connect to License Server at: " + serverIp);
                TryAndCheckOutFloatingLicense(serverIp);

            }
            else
            {
                Console.WriteLine("License Not Found. Contact with Machine ID");
                licenseType = LicenseType.None;
                Console.ReadLine();
            }
        }

        private static bool CheckNodeLockedLicense()
        {
            LicenseType _licenseType;
            string _name, _email;

            bool _validLicense = LicenseManager.CheckLicenseStatus(out _licenseType, out _name, out _email);

            if (_validLicense)
            {
                Console.WriteLine("Node-Locked License: Name: " + _name + " Email: " + _email);
                return true;
            }
            else
            {
                Console.WriteLine("Invalid License.");
                return false;
            }

        }

        public static void StartManagerToSlaveServer()
        {
            IPAddress ipAddress = GetLocalIPAddress();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11002);
            Console.WriteLine("Server IP Address: " + ipAddress);

            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("RoboAnalyzer can be started...");
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
                Environment.Exit(0);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void TryAndCheckOutFloatingLicense(string serverIp)
        {
            try
            {
                IPAddress _serverIPAddress = IPAddress.Parse(serverIp);
                IPEndPoint remoteEP = new IPEndPoint(_serverIPAddress, 11001);

                using (managerToServerSocket = new Socket(_serverIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    managerToServerSocket.Connect(remoteEP);
                    Console.WriteLine("Connected to License Server.");

                    // Send client ID to server
                    string clientId = LicenseManager.GetMacAddress(); // Generate or define your client ID logic here
                    //byte[] msgClientID = Encoding.ASCII.GetBytes(clientId);
                    //client.Send(msgClientID);

                    string _localIPAddress = GetLocalIPAddress().ToString();


                    byte[] _msgClientIDAndLocalIPAddress = Encoding.ASCII.GetBytes(clientId + "," + _localIPAddress);
                    managerToServerSocket.Send(_msgClientIDAndLocalIPAddress);

                    //Console.WriteLine("Client ID sent to License Server: " + clientId);

                    // Start listening for responses from the server
                    Thread receiveThread = new Thread(() => ReceiveMessages(managerToServerSocket));
                    receiveThread.Start();

                    // Simulate client activity
                    Console.WriteLine("Client is active.");
                    StartManagerToSlaveServer();
                    Console.ReadKey();

                    // Close the connection
                    managerToServerSocket.Shutdown(SocketShutdown.Both);
                    managerToServerSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);

                Environment.Exit(0);

            }
        }

        private static void TryAndCheckInFloatingLicense()
        {
            try
            {
                if (managerToServerSocket != null)
                {
                    string clientId = LicenseManager.GetMacAddress(); // Generate or define your client ID logic here
                    byte[] _msgClientID = Encoding.ASCII.GetBytes(clientId);
                    managerToServerSocket.Send(_msgClientID);

                    // Close the connection
                    managerToServerSocket.Shutdown(SocketShutdown.Both);
                    managerToServerSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);

                Environment.Exit(0);

            }
        }

        private static void ReceiveMessages(Socket client)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = client.Receive(buffer);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    Console.WriteLine("Message from License Server: " + message);

                    if (message == "LicensesLimitReached")
                    {
                        Console.WriteLine("Try the check-out licenses once the license pool is free. Press any key to exit.");
                        Thread.Sleep(10000); // 10000 milliseconds = 10 seconds

                        // Close the application
                        Environment.Exit(0);

                        // Close the connection
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Environment.Exit(0);

            }
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
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());

                Environment.Exit(0);


                // Close the application


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

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            TryAndCheckInFloatingLicense();
            Console.WriteLine("exit");
        }
    }
}
