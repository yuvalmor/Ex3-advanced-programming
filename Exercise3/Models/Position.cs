using System.Xml;

namespace Exercise3.Models
{
    /*
     *  The class Position responsible to store the values of the plane position.
     */
    public class Position
    {
        // Plane longitude
        public double Lon { get; set; }
        // Plane latitude 
        public double Lat { get; set; }
        // Plane throttle
        public double Rudder { get; set; }
        // Plane rudder
        public double Throttle { get; set; }

        /*
         * The function NewPosToXml - write the current position of the plane to xml file.
         */
        public void NewPosToXml(XmlWriter writer)
        {
            writer.WriteStartElement("NewPos");
            writer.WriteElementString("NewLon", this.Lon.ToString());
            writer.WriteElementString("NewLat", this.Lat.ToString());
            writer.WriteEndElement();
        }

        /*
         * The function LastPosToXml- write the previous position of the plane to xml file.
         */
        public void LastPosToXml(XmlWriter writer)
        {
            writer.WriteStartElement("LastPos");
            writer.WriteElementString("LastLon", this.Lon.ToString());
            writer.WriteElementString("LastLat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}