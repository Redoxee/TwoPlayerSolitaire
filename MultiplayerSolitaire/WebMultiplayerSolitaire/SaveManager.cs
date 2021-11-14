namespace MSGWeb
{
    public class SaveManager
    {
        private static SaveManager instance;

        public static SaveManager Instance => SaveManager.instance;

        private const string SaveNamePattern = "TwoPlayerSolitair_{0}_{1}.save";

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
            this.gameDateName = System.DateTime.UtcNow.ToString("yyyy_mm_dd");
            this.gameSaveInstanceCount = 0;
        }

        public void RequestSave()
        {
            MSG.Sandbox sandbox = GameProcess.Instance.GetGameManager().GetSandbox();

            string saveFullPath = $"{this.savePath}/{string.Format(SaveManager.SaveNamePattern, this.gameDateName, this.gameSaveInstanceCount)}";
            System.Console.WriteLine($"Saving at {saveFullPath}");
            this.gameSaveInstanceCount++;

            System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFullPath);
            AMG.Serializer serializer = new AMG.Serializer();
            serializer.StartWrite(writer);
            sandbox.Serialize(serializer);
            writer.Close();
        }
    }
}
