using System;
namespace MambuStreamingReader.entities
{
    public class Entity
    {
        public enum EnityType { client, deposit, transaction };
        public string id { get; set; }
        public EnityType type { get; set; }
    }
}
