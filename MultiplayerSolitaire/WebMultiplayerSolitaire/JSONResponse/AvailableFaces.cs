namespace MSGWeb
{
    public class AvailableFaces : JSONResponse
    {
        public override string MessageType => "AvailableFaces";

        public bool[] Faces;
        public bool ReadyToPlay;
    }
}
