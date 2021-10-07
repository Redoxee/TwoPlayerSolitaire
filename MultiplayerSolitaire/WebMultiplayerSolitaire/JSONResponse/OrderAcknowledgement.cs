using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMultiplayerSolitaire
{
    public class OrderAcknowledgement : JSONResponse
    {
        public override string MessageType => "OrderAcknowledgement";
        public int OrderID;
        public int PlayerIndex;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MSG.Failures FailureFlags;
    }
}
