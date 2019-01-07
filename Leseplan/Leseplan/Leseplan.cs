using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leseplan
{
    public class LeseplanData
    {
        public static LeseplanData Load(string json)
        {
            return JsonConvert.DeserializeObject<LeseplanData>(json);
        }

        public string Id { get; set; }
        public List<EntryData> Entries { get; set; } = new List<EntryData>();
    }

    public class EntryData
    {
        public List<ItemData> Items { get; set; } = new List<ItemData>();
    }

    public class ItemData
    {
        /// <summary>
        /// Bible reference
        /// </summary>
        public string Ref { get; set; }
    }

    public class LeseplanUserData
    {
        public static LeseplanUserData Load(string json)
        {
            return JsonConvert.DeserializeObject<LeseplanUserData>(json);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public string Translation { get; set; } = "LUT";

        public Dictionary<string, ItemReadData> ReadItems { get; set; } = new Dictionary<string, ItemReadData>(StringComparer.Ordinal);
    }

    public class ItemReadData
    {
        public string Ref { get; set; }
        public bool Checked { get; set; }
    }

}
