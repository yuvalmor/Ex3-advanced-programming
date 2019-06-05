using Exercise3.Models;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
namespace Exercise3.Controllers
{
    public class FlightController : Controller
    {
        // Opening window
        public ActionResult Index()
        {
            return View();
        }

        /*
         * The function display responsible to display three diffrent situations:
         * The current location of the plane (if time parameter is zero).
         * The live track of the plane, sample the plane location according to the parameter.
         * The reconstruction treck of the plane according to file that hold the data.
         */
        [HttpGet]
        public ActionResult Display(string ip, int port, int? time = 0)
        {
            System.Net.IPAddress validIp = null;
            ViewBag.readFromFile = 0;
            // Check if the given parameter is valid ip address
            if (System.Net.IPAddress.TryParse(ip, out validIp))
            {
                // Connect to the simulator server with the appropriate ip and port
                FlightModel.Instance.InitialClient(ip, port);
                ViewBag.time = time;
            }
            else
            {
                // update the variables according to the third option
                ViewBag.readFromFile = 1;
                ViewBag.time = port;
                ViewBag.fileName = ip;
            }
            return View();
        }


        /*
         * Show the plane track and save its: longitude, latitude, throttle and rudder values in evry sample.
         * Sample and display the data in the given frequancy (number of times per second) for the given duration.
         */
        [HttpGet]
        public ActionResult Save(string ip, int port, int frequency,int duration, string fileName)
        {
            // Conect to the simulator server
            FlightModel.Instance.InitialClient(ip, port);
            // Save the parameters
            FlightModel.Instance.SetNumSamples(duration * frequency); 
            ViewBag.frequency = frequency;
            ViewBag.duration = duration;
            ViewBag.fileName = fileName;
            return View();
        }


        /*
         *  GetPlaneLocation - take the longitude, latitude, throttle and rudder values from the Simulator.
         *  It return the plane location in xml form.
         */
        public string GetPlaneLocation()
        {
            // Connect to the server
            Client client = FlightModel.Instance.GetClient();
            // Get the appropriate request
            string latLine = client.GetRequestToSimulator("latitude");
            string lonLine = client.GetRequestToSimulator("longitude");
            string rudderLine = client.GetRequestToSimulator("rudder");
            string throttleLine = client.GetRequestToSimulator("throttle");
            // Get the values from the server
            string lat = client.GetInfo(latLine);
            string lon = client.GetInfo(lonLine);
            string rudder = client.GetInfo(rudderLine);
            string throttle = client.GetInfo(throttleLine);
            // Create the current position
            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);
            position.Rudder = Double.Parse(rudder);
            position.Throttle = Double.Parse(throttle);
            // Save the position in the list
            lock (FlightModel.Instance.getLock())
            {
                FlightModel.Instance.GetPositions().Add(position);
            }
            // Save the data into xml file
            return PosToXml(position);
        }


        /*
         * PosToXml -  Write the position to xml file
         */
        private string PosToXml(Position position)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            // Write the data into the xml file
            writer.WriteStartDocument();
            writer.WriteStartElement("NewPos");
            position.NewPosToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /*
         *  GetPlaneTreck - get the current position from the server, 
         *  And the last position from the list.
         *  It return the data in xml form.
         */
        public string GetPlaneTreck()
        {
            Client client = FlightModel.Instance.GetClient();
            // Get the appropriate request
            string latLine = client.GetRequestToSimulator("latitude");
            string lonLine = client.GetRequestToSimulator("longitude");
            string rudderLine = client.GetRequestToSimulator("rudder");
            string throttleLine = client.GetRequestToSimulator("throttle");
            // Get the values from the server
            string lat, lon, rudder, throttle;
            lat = client.GetInfo(latLine);
            lon = client.GetInfo(lonLine);
            rudder = client.GetInfo(rudderLine);
            throttle = client.GetInfo(throttleLine);
            // Create the current position
            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);
            position.Rudder = Double.Parse(rudder);
            position.Throttle = Double.Parse(throttle);
            // Get the last position and push the current position
            Position lastPosition;
            lock (FlightModel.Instance.getLock())
            {
                lastPosition = FlightModel.Instance.GetPositions().Last();
                FlightModel.Instance.GetPositions().Add(position);
            }
            // Save the data into xml file
            return TreckToXml(position, lastPosition);
        }


        private string TreckToXml(Position newPosition, Position lastPossition)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            // Write the data into the xml file
            writer.WriteStartDocument();
            writer.WriteStartElement("Treck");
            newPosition.NewPosToXml(writer);
            lastPossition.LastPosToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        /*
         * GetPositionsFromFile - parse the data from the file into one line using the function ReadData.
         */
        public string GetPositionsFromFile(string fileName)
        {
            string positions = FlightModel.Instance.ReadData(fileName);
            return positions;
        }

        /*
         * SaveDataToFile - Save the plane track into txt file using the function WriteData.
         */
        public int SaveDataToFile(string fileName)
        {
            // Check that the sampeling process end befor writting
            if (FlightModel.Instance.GetPositions().Count >= FlightModel.Instance.GetNumSamples())
            {
                FlightModel.Instance.WriteData(fileName);
                return 1;
            }
            return 0;
        }
    }
}