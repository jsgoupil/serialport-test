using RJCP.IO.Ports;
using System;
using System.Text;

namespace serialport_test
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Write the portName as argument.");
                return;
            }

            Console.WriteLine("Starting on port " + args[0]);
            var serialPort = new RJCP.IO.Ports.SerialPortStream(args[0])
            {
                BaudRate = 19200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                ReadTimeout = 1000,
                WriteTimeout = 500,
                Handshake = Handshake.None
            };

            if (!serialPort.IsOpen)
            {
                Console.WriteLine("Opening port.");
                serialPort.Open();
            }

            Console.WriteLine("Press ESCAPE to read a line.");
            Console.WriteLine("Otherwise, type and perss enter to send a message.");
            Console.WriteLine("Type exit to quit.");
            var buffer = new StringBuilder();
            do
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    string read = null;
                    try
                    {
                        read = serialPort.ReadLine();
                    }
                    catch (TimeoutException)
                    {
                        read = "TIMEOUT";
                    }
                    Console.WriteLine("R> " + read);
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    var final = buffer.ToString();
                    if (final.ToUpper() == "EXIT")
                    {
                        break;
                    }

                    serialPort.WriteLine(final);
                    Console.WriteLine("");
                    buffer.Clear();
                }
                else
                {
                    buffer.Append(key.KeyChar);
                }
            } while (true);

            serialPort.Close();

            Console.WriteLine("Ending");
        }
    }
}
