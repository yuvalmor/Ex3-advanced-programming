using System.Net.Sockets;
using System.Text;

namespace Exercise3.Models
{
    public class Client
    {
        private TcpClient client;
        // ctor
        public Client()
        {
            client = new TcpClient();
        }

        /*
         * This function sends connect request to the server (simulator)
         * repeatedly until connection is made or interupted
         */
        public void Connect(string ip, int port)
        {
            while (true)
            {
                try
                {
                    client.Connect(ip, port);
                    break;
                }
                catch { }
            }
        }

        /*
         * This function prepeares a request command for the simulator
         * based on given field
         */
        public string GetRequestToSimulator(string kind)
        {
            string request = "get ";
            switch (kind)
            {
                case "longitude":
                    request += Consts.LONGIDUTE_XML;
                    break;
                case "latitude":
                    request += Consts.LATITUDE_XML;
                    break;
                case "rudder":
                    request += Consts.RUDDER_XML;
                    break;
                case "throttle":
                    request += Consts.THROTTLE_XML;
                    break;
                default:
                    return "";

            }

            request += Consts.NEW_LINE;
            return request;
        }

        /*
         * This function sends command to the server and reads the response
         * which is the value of the field requested and returns it
         */
        public string GetInfo(string request)
        {
            string info = "";
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] buffer = asen.GetBytes(request);
                NetworkStream stream = client.GetStream();

                byte[] readBytes;
                int sizeRead;
                lock (FlightModel.Instance.getLock())
                {
                    stream.Write(buffer, 0, buffer.Length);
                    readBytes = new byte[client.ReceiveBufferSize];
                    sizeRead = stream.Read(readBytes, 0, client.ReceiveBufferSize);
                }

                if (sizeRead > 0)
                {
                    info = System.Text.Encoding.UTF8.GetString(readBytes, 0, readBytes.Length);
                    string[] splitInfo = info.Split('\'');
                    info = splitInfo[Consts.INFO_POSITION];
                }

            }
            catch { }
            return info;
        }
    }
}