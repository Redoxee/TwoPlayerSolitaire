using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGWeb
{
    internal abstract class JSONResponse
    {
        public abstract string MessageType { get; }
    }
}
