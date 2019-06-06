using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Exercise3.Models
{
    /*
     * The class FlightModel - holds all the relevant information for the flight.
     * Singleton class
     */
    public class FlightModel
    {
        // Lock the access for the position list
        private readonly object locker = new object();
        // List that holds the plane position (track)
        private List<Position> positions;
        // Client that connect to the simulator server
        private Client client;
        // Variable that hold the number of times we need to sampke the plane track
        private int numSamples;
        
        // Getter for the position list
        public List<Position> GetPositions()
        {
            return this.positions;
        }

        // Getter for the lock
        public object getLock()
        {
            return this.locker;
        }

        // Getter for the variable that holds the number of samples
        public int GetNumSamples()
        {
            return this.numSamples;
        }

        // Setter for the variable that holds the number of samples
        public void SetNumSamples(int num)
        {
            this.numSamples = num;
        }

        // Getter for the client
        public Client GetClient()
        {
            return this.client;
        }
        
        // Singleton class
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

        // Constructor, initial the position list
        public FlightModel()
        {
            this.positions = new List<Position>();
        }

        // Create connection to the server 
        public void InitialClient(string ip, int port)
        {
            client = new Client();
            client.Connect(ip, port);
        }

        /*
         * The function ReadData - read the data from the given file name,
         * And Parse it into one line of string. 
         */
        public string ReadData(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(Consts.SCENARIO_FILE, fileName));
            // split the files int lines
            string[] lines = File.ReadAllLines(path);
            string data = "";
            string[] temp;
            // read evry line and save the relevant data (lon + lat)
            for (int i = 0; i < lines.Length; i++)
            {
                // split the line into tokens
                temp = lines[i].Split(',');
                data += temp[0];
                data += ",";
                data += temp[1];
                data += ",";
            }
            return data;
        }


        /*
         * The function WriteData - write the plane track into file with the given name.
         */
        public void WriteData(string fileName)
        {
            // Get the path to the file
            string path = HttpContext.Current.Server.MapPath(String.Format(Consts.SCENARIO_FILE, fileName));
            string line;
            // Create the file
            StreamWriter file = new StreamWriter(path);
            // Read all the positions in the list, and write the data of each position into the file
            for (int i = 0; i < positions.Count; i++)
            {
                line = positions[i].Lon + "," + positions[i].Lat +
                    "," + positions[i].Rudder + "," + positions[i].Throttle;
                file.WriteLine(line);
            }
            file.Close();
        }
    }
}