using System.Net.Sockets;
using System.Text;

namespace Exercise3.Models
{
    public class Client
    {
        private static Client s_instace = null;
        private static TcpClient client;
        private bool connected = false;

        public static Client Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new Client();
                    client = new TcpClient();
                }
                return s_instace;
            }
        }


        public void Connect(string ip, int port)
        {
            while (!connected)
            {
                try
                {
                    client.Connect(ip, port);
                    connected = true;
                    break;
                }
                catch { }
            }
        }

        public string GetLine(string kind)
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
        public string GetData(string line)
        {
            string info = "";
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] buffer = asen.GetBytes(line);
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);

                byte[] readBytes = new byte[client.ReceiveBufferSize];
                int sizeRead = stream.Read(readBytes, 0, client.ReceiveBufferSize);
                if (sizeRead > 0)
                {
                    info = System.Text.Encoding.UTF8.GetString(readBytes, 0, readBytes.Length);
                    string[] splitInfo = info.Split('\'');
                    info = splitInfo[1];
                }
                else
                {
                    // print message?
                }

            }
            catch { }
            return info;

        }

    }
}