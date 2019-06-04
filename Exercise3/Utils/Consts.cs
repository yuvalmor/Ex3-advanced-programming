
namespace Exercise3.Models
{
    public class Consts
    {
        public const int BUFFER_SIZE = 1024;
        public const int READ_FAILED = -1;
        public const int INFO_POSITION = 1;
        public const string LON_XML = "longitude-deg";
        public const string LAT_XML = "latitude-deg";
        public const string THROTTLE_XML = "/controls/engines/current-engine/throttle";
        public const string RUDDER_XML = "/controls/flight/rudder";
        public const string LATITUDE_XML = "/position/latitude-deg";
        public const string LONGIDUTE_XML = "/position/longitude-deg";
        public const string NEW_LINE = "\r\n";
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";
    }
}