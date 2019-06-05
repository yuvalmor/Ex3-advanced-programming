using Exercise3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [HttpGet]
        public ActionResult Save(string ip, int port, int frequency,int duration, string fileName)
        {
            FlightModel.Instance.InitialClient(ip, port);
            ViewBag.frequency = frequency;
            ViewBag.duration = duration;
            ViewBag.fileName = fileName;
            return View();
        }

        public string GetPlaneLocation()
        {
            Client client = FlightModel.Instance.GetClient();
            string latLine = client.GetRequestToSimulator("latitude");
            string lonLine = client.GetRequestToSimulator("longitude");
            string rudderLine = client.GetRequestToSimulator("rudder");
            string throttleLine = client.GetRequestToSimulator("throttle");

            string lat = client.GetInfo(latLine);
            string lon = client.GetInfo(lonLine);
            string rudder = client.GetInfo(rudderLine);
            string throttle = client.GetInfo(throttleLine);

            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);
            position.Rudder = Double.Parse(rudder);
            position.Throttle = Double.Parse(throttle);

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
            string rudderLine = client.GetRequestToSimulator("rudder");
            string throttleLine = client.GetRequestToSimulator("throttle");

            string lat, lon, rudder, throttle;
            lat = client.GetInfo(latLine);
            lon = client.GetInfo(lonLine);
            rudder = client.GetInfo(rudderLine);
            throttle = client.GetInfo(throttleLine);
            
            Position position = new Position();
            position.Lat = Double.Parse(lat);
            position.Lon = Double.Parse(lon);
            position.Rudder = Double.Parse(rudder);
            position.Throttle = Double.Parse(throttle);

            Debug.WriteLine("check");
            Debug.WriteLine(position.Lat);
            Debug.WriteLine(position.Lon);
            Debug.WriteLine(position.Rudder);
            Debug.WriteLine(position.Throttle);

            Position lastPosition;
            lock (FlightModel.Instance.getLock())
            {
                lastPosition = FlightModel.Instance.GetPositions().Last();
                FlightModel.Instance.GetPositions().Add(position);
            }
            
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

        public string GetPositionsFromFile(string fileName)
        {
            string positions = FlightModel.Instance.ReadData(fileName);
            return positions;
        }

        public void SaveDataToFile(string fileName)
        {
            FlightModel.Instance.WriteData(fileName);
        }
    }
}