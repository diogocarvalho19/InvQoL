using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvQoL.Utils
{
    public class InventoryItem
    {
        public Item Item { get; set; }
        public byte SizeX { get; set; }
        public byte SizeY { get; set; }
        public byte Rot { get; set; }
    }
}
