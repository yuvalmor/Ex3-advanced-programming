using Exercise3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Exercise3.Controllers
{
    public class FlightController : Controller
    {
        // GET: Flight
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Display(string ip, int port, int? time = 0)
        {
            System.Net.IPAddress validIp = null;
            ViewBag.readFromFile = 0;
            if (System.Net.IPAddress.TryParse(ip, out validIp))
            {
                FlightModel.Instance.InitialClient(ip, port);
                ViewBag.time = time;
            }
            else
            {
                ViewBag.readFromFile = 1;
                ViewBag.time = port;
                ViewBag.fileName = ip;
            }
            return View();
        }

        public string GetPlaneLocation()
        {
            Client client = FlightModel.Instance.GetClient();
            string latLine = client.GetRequestToSimulator("latitude");
            string lonLine = client.GetRequestToSimulator("longitude");

            string lat = client.GetInfo(latLine);
            string lon = client.GetInfo(lonLine);

            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);

            FlightModel.Instance.GetPositions().Add(position);

            return PosToXml(position);
        }

        private string PosToXml(Position position)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("NewPos");
            position.NewPosToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public string GetPlaneTreck()
        {
            Client client = FlightModel.Instance.GetClient();

            string latLine = client.GetRequestToSimulator("latitude");
            string lonLine = client.GetRequestToSimulator("longitude");

            string lat = client.GetInfo(latLine);
            string lon = client.GetInfo(lonLine);

            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);
            
            Position lastPosition = FlightModel.Instance.GetPositions().Last();
            FlightModel.Instance.GetPositions().Add(position);

            return TreckToXml(position, lastPosition);
        }

        private string TreckToXml(Position newPosition, Position lastPossition)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Treck");
            newPosition.NewPosToXml(writer);
            lastPossition.LastPosToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        [HttpPost]
        public string GetPositionsFromFile(string fileName)
        {
            string positions = FlightModel.Instance.ReadData(fileName);
            Console.WriteLine(positions);
            return positions;
        }

    }
}