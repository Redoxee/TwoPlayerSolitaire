namespace MSGWeb
{
    public class SaveManager
    {
        private static SaveManager instance;

        public static SaveManager Instance => SaveManager.instance;

        private const string SaveNamePattern = "TwoPlayerSolitaire_{0}_{1}.save";

        private string savePath;
        private string gameDateName;
        private int gameSaveInstanceCount;

        public static void Initialize(ref MSGWeb.Parameters parameters)
        {
            SaveManager saveManager = new SaveManager(ref parameters);
            SaveManager.instance = saveManager;
        }

        private SaveManager(ref MSGWeb.Parameters parameters)
        {
            this.savePath = parameters.SavePath;
            this.gameDateName = AMG.DateNameConversion.NameDate(System.DateTime.Now);
            this.gameSaveInstanceCount = 1;
        }

        public string RequestSave()
        {
            if (string.IsNullOrEmpty(this.savePath))
            {
                System.Console.WriteLine("No save path configured.");
                return "No save path configured.";
            }

            MSG.Sandbox sandbox = GameProcess.Instance.GetGameManager().GetSandbox();

            string saveFullPath = $"{this.savePath}/{string.Format(SaveManager.SaveNamePattern, this.gameDateName, this.gameSaveInstanceCount)}";
            System.Console.WriteLine($"Saving at {saveFullPath}");
            this.gameSaveInstanceCount++;

            System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFullPath);
            AMG.Serializer serializer = new AMG.Serializer();
            serializer.StartWrite(writer);
            sandbox.Serialize(serializer);
            writer.Close();
            return $"Game saved at {saveFullPath}";
        }
    }
}
