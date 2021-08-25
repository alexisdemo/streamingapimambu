using System;
namespace MambuStreamingReader.entities
{
    public class Event
    {
        public Metadata metadata { get; set; }
        public string template_name { get; set; }
        public string category { get; set; }
        public dynamic body { get; set; }
    }
}
