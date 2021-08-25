using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerSolitaireGame
{
    public enum GameStateID : short
    {
        Initialize,
        Betting,
        Fold,
        EndGame,
        Unkown,
    }
}
