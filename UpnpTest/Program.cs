using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rssdp;
using RestSharp;
using System.Threading.Tasks;

namespace UpnpTest
{
    class Program
    {
        static string host = "192.168.1.115";
        static string port = "8080";
        static string protocol = "roap"; // could also be hdcp
        static string apiCommand = "api/command";
        static string authCommand = "api/auth";
        static string sessionID = "212440449";

        static List<SsdpDevice> founddevicelist = new List<SsdpDevice>();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            //Find TV and port on Network
            var task = SearchForDevices();
            task.Wait();
            var foundTV = false;
            string foundTVAddress;
            foreach (var device in founddevicelist)
            {
                if (device.Manufacturer == "LG")
                {
                    Console.WriteLine("Found TV");
                    Console.WriteLine("TV IP: {0}", device.ModelUrl);
                    foundTV = true;
                    foundTVAddress = device.ModelUrl.ToString();
                }
            }
            if (foundTV == false)
            {
                Console.WriteLine("Can't find tv");
            }
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();

            //Determine if session ID exists
            //If not do Auth Method

            Console.WriteLine("What Command do you want to send?");
            var commandRead = Console.ReadLine();

            //Send command
            var command = new KeyInputClass()
            {
                session = sessionID,
                type = "HandleKeyInput",
                value = int.Parse(commandRead)
            };

            var response = SendCommand(command, CommandType.Keyinput);
            Console.WriteLine(response);

            Console.WriteLine("Press Any Key to End Program");
            Console.ReadKey();
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
                    // Device data returned only contains basic device details and location ]
                    // of full device description.
                    Console.WriteLine("Found " + foundDevice.Usn + " at " + foundDevice.DescriptionLocation.ToString());

                    // Can retrieve the full device description easily though.
                    var fullDevice = await foundDevice.GetDeviceInfo();
                    founddevicelist.Add(fullDevice);
                    Console.WriteLine(fullDevice.FriendlyName);
                    Console.WriteLine();
                }
                Console.WriteLine("Completed Search");
            }
            return founddevicelist;
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
                requestLocation = string.Format("{0}/{1}",protocol, authCommand);
            }
            else
            {
                requestLocation = string.Format("{0}/{1}", protocol, apiCommand);
            }
            var fullHost = string.Format("http://{0}:{1}", host, port);
            var client = new RestClient(fullHost);
            var request = new RestRequest(requestLocation, Method.POST);
            request.AddHeader("Content-Type", "application/atom+xml");
            request.AddBody(body);
            var response = client.Execute(request);
            return response.Content;
        }



    }
}
