using System;
using System.Collections.Generic;

namespace MambuStreamingReader.entities
{
    public class StreamData
    {
        public IList<Event> events { get; set; }
        public Cursor cursor { get; set; }
        public dynamic info { get; set; }
    }
}
