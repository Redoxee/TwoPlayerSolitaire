using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMultiplayerSolitaire
{
    public class AvailablePlayerSlot : JSONResponse
    {
        public override string MessageType => "AvailablePlayerSlots";

        public bool[] AvaialablePlayerSlots = null;
    }
}
