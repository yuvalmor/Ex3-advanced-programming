using System.Xml;

namespace Exercise3.Models
{
    public class Position
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }

        public void NewPosToXml(XmlWriter writer)
        {
            writer.WriteStartElement("NewPos");
            writer.WriteElementString("NewLon", this.Lon.ToString());
            writer.WriteElementString("NewLat", this.Lat.ToString());
            writer.WriteEndElement();
        }
        public void LastPosToXml(XmlWriter writer)
        {
            writer.WriteStartElement("LastPos");
            writer.WriteElementString("LastLon", this.Lon.ToString());
            writer.WriteElementString("LastLat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}