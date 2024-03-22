using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SimpleCalculatorApplication
{
    public partial class SimpleCalculatorCmdDlg : Form
    {
        private double numberA;
        private double numberB;
        private double result;
        private bool isvalid = false;
        private TcpClient client = null;
        private Timer pingTimer;

        public SimpleCalculatorCmdDlg()
        {
            InitializeComponent();
            StartClient(); // Start the client connection
            CheckLicense(); // Check the license validity

            pingTimer = new Timer();
            pingTimer.Interval = 10000; // Set interval to 10 seconds (10000 milliseconds)
            pingTimer.Tick += new EventHandler(PingLicenseServer);
            pingTimer.Start();

        }

        private void CheckLicense()
        {
            if (!isvalid)
            {
                MessageBox.Show("Invalid license. Please contact support.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0); // Exit the application if the license is invalid
            }
        }
        public void StartClient()
        {
            byte[] bytes = new byte[1024];
            try
            {
                // Enter the IP address of the license server

                string licenseServerIP = GetLocalIPAddress().ToString(); ;
                int licenseServerPort = 11002; // Port number your license server is listening on

                // Initialize the client to connect to the license server
                client = new TcpClient(licenseServerIP, licenseServerPort);
                Console.WriteLine("Connected to license server.");
                isvalid = true; // Set the license validation flag
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void PingLicenseServer(object sender, EventArgs e)
        {
            try
            {
                if (!IsConnected(client))
                {
                    Console.WriteLine("Connection to license server closed. Exiting application.");
                    Environment.Exit(0); // Exit the application if the connection to the license server is closed
                }

                // Send ping message to the license server
                string pingMessage = "Ping<EOF>"; // Customize ping message as per your protocol
                byte[] data = Encoding.ASCII.GetBytes(pingMessage);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Ping sent to license server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending ping to license server: " + ex.Message);
            }
        }

        private bool IsConnected(TcpClient client)
        {
            try
            {
                // Check if the client socket is connected
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    // Poll the socket for readability
                    return !(client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0);
                }
                return false;
            }
            catch
            {
                return false;
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

        private void cmdSum_Click(object sender, EventArgs e)
        {
            GetUserInput();
            result = numberA + numberB;
            ShowResult();
        }

        private void cmdSubtract_Click(object sender, EventArgs e)
        {
            GetUserInput();
            result = numberA - numberB;
            ShowResult();
        }

        private void cmdMultiply_Click(object sender, EventArgs e)
        {
            GetUserInput();
            result = numberA * numberB;
            ShowResult();
        }

        private void cmdDivide_Click(object sender, EventArgs e)
        {
            GetUserInput();
            result = numberA / numberB;
            ShowResult();
        }

        private void GetUserInput()
        {
            numberA = Convert.ToDouble(txtNumberA.Text);
            numberB = Convert.ToDouble(txtNumberB.Text);
        }

        private void ShowResult()
        {
            txtResult.Text = result.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SimpleCalculatorCmdDlg_Load(object sender, EventArgs e)
        {

        }
    }
}
