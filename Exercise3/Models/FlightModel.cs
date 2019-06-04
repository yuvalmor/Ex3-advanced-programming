using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;



namespace Exercise3.Models
{
    public class FlightModel
    {
        private Client client;
        private List<Position> positions;

        public List<Position> GetPositions()
        {
            return this.positions;
        }

        public Client GetClient()
        {
            return this.client;
        }
        private static FlightModel s_instace = null;
        public static FlightModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new FlightModel();
                    
                }
                return s_instace;
            }
        }

        public FlightModel()
        {
            positions = new List<Position>();
        }

        public void InitialClient(string ip, int port)
        {
            client = new Client();
            client.Connect(ip, port);
        }

        public string ReadData(string fileName)
        {

            Debug.WriteLine(fileName);

            string path = HttpContext.Current.Server.MapPath(String.Format(Consts.SCENARIO_FILE, fileName));
            string[] lines = System.IO.File.ReadAllLines(path);
            string data = "";
            string[] temp;
            for (int i = 0; i < lines.Length; i++)
            {
                temp = lines[i].Split(',');
                data += temp[0];
                data += ",";
                data += temp[1];
                data += ",";
            }
            Debug.WriteLine(data);
            return data;
        }
    }
}