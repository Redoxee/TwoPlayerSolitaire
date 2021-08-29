namespace WebMultiplayerSolitaire
{
    public class SandboxChanges : JSONResponse
    {
        public override string MessageType => nameof(SandboxChanges);

        public MultiplayerSolitaireGame.GameChange[] GameChanges;
        public PlayerViewUpdate PlayerViewUpdate;
    }
}
