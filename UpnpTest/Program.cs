using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Rssdp;
using RestSharp;
using System.Threading.Tasks;

namespace UpnpTest
{
    class Program
    {
        private const string Host = "192.168.1.115";
        private const string Port = "8080";
        private const string Protocol = "roap"; // could also be hdcp
        private const string ApiCommand = "api/command";
        private const string AuthCommand = "api/auth";
        private const string SessionId = "212440449";

        static readonly List<SsdpDevice> FoundDeviceList = new List<SsdpDevice>();

        static void Main()
        {
            
            Console.WriteLine("Starting");

            var deviceIp = SearchForDevice("47LM6700");
            deviceIp.Wait();

            if (deviceIp.Result != "")
            {
                Console.WriteLine("TV Found and IP is {0}", deviceIp.Result);
            }

            //Determine if session ID exists
            //If not do Auth Method
            
            while (true)
            {
                Console.WriteLine("What Command do you want to send? Type exit to exit");
                var commandRead = Console.ReadLine();

                if (commandRead != null && commandRead.ToLowerInvariant() == "exit")
                {
                    return;
                }

                //Send command
                var command = new KeyInputClass
                {
                    session = SessionId,
                    type = "HandleKeyInput",
                    value = int.Parse(commandRead)
                };

                var response = SendCommand(command, CommandType.Keyinput);
                Console.WriteLine(response);
            }
        }

        public static string SendKeyCommand(Commands.CommandCodeList commandCode)
        {
            //Send command
            var command = new KeyInputClass
            {
                session = SessionId,
                type = "HandleKeyInput",
                value = commandCode.GetHashCode()
            };

            return SendCommand(command, CommandType.Keyinput);
        }

        private static async Task<string> SearchForDevice(string deviceName)
        {
            await SearchForDevices();

            foreach (var device in FoundDeviceList)
            {
                if (device.FriendlyName.Contains(deviceName))
                {
                    return device.ModelUrl.ToString();
                }
            }

            Console.WriteLine("Can't find tv");
            return "";
        }

        /// <summary>
        /// Searches for all Upnp Devices on the network
        /// </summary>
        /// <returns>List of devices found</returns>
        public static async Task<List<SsdpDevice>> SearchForDevices()
        {
            // This code goes in a method somewhere.
            using (var deviceLocator = new SsdpDeviceLocator())
            {
                var foundDevices = await deviceLocator.SearchAsync(); // Can pass search arguments here (device type, uuid). No arguments means all devices.
                
                foreach (var foundDevice in foundDevices)
                {
                    try
                    {
                        // Device data returned only contains basic device details and location ]
                        // of full device description.
                        Console.WriteLine("Found " + foundDevice.Usn + " at " + foundDevice.DescriptionLocation);

                        // Can retrieve the full device description easily though.
                        var fullDevice = await foundDevice.GetDeviceInfo();
                        FoundDeviceList.Add(fullDevice);
                        Console.WriteLine(fullDevice.FriendlyName);
                        Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine( "Exception Thrown");
                    }
                    
                }
                Console.WriteLine("Completed Search");
            }
            return FoundDeviceList;
        }

        public enum CommandType
        {
            Authorization,
            Keyinput
        }
        
        public static string SendCommand(object body, CommandType commandType )
        {
            string requestLocation;
            if (commandType == CommandType.Authorization)
            {
                requestLocation = string.Format("{0}/{1}",Protocol, AuthCommand);
            }
            else
            {
                requestLocation = string.Format("{0}/{1}", Protocol, ApiCommand);
            }
            var fullHost = string.Format("http://{0}:{1}", Host, Port);
            var client = new RestClient(fullHost);
            var request = new RestRequest(requestLocation, Method.POST);
            request.AddHeader("Content-Type", "application/atom+xml");
            request.AddBody(body);
            var response = client.Execute(request);
            return response.Content;
        }

        public static bool IsTvOn()
        {
            var pinger = new Ping();
            var result = pinger.Send(Host, 500);
            if (result != null && result.Status == IPStatus.Success)
            {
                //Check if find TV Name
            }
            return false;
        }

    }
}
