﻿namespace MSGWeb
{
    public class AvailablePlayerSlot : JSONResponse
    {
        public override string MessageType => "AvailablePlayerSlots";

        public bool[] AvaialablePlayerSlots = null;
    }
}
