using System;
namespace MambuStreamingReader.entities
{
    public class Cursor
    {
        public string event_type { get; set; }
        public string cursor_token { get; set; }
        public string offset { get; set; }
        public string partition { get; set; }
    }
}
