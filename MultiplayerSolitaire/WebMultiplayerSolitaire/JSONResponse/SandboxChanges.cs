namespace MSGWeb
{
    public class SandboxChanges : JSONResponse
    {
        public override string MessageType => nameof(SandboxChanges);

        public MSG.GameChange[] GameChanges;
    }
}
