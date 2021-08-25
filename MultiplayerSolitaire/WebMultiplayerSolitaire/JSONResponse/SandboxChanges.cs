using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMultiplayerSolitaire
{
    public class SandboxChanges : JSONResponse
    {
        public override string MessageType => nameof(SandboxChanges);

        public MultiplayerSolitaireGame.GameChange[] GameChanges;
        public PlayerViewUpdate PlayerViewUpdate;
    }
}
