using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMultiplayerSolitaire
{
    public abstract class JSONResponse
    {
        public abstract string MessageType { get; }
    }
}
