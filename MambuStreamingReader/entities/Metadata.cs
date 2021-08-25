using System;
namespace MambuStreamingReader.entities
{
    public class Metadata
    {
        public string eid { get; set; }
        public string occurred_at { get; set; }
        public string content_type { get; set; }
        public string category { get; set; }
        public string event_type { get; set; }
    }
}
