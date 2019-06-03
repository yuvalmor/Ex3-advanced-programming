using System.Collections.Generic;

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
    }
}