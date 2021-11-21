namespace MSGWeb
{
    using System.Resources;
    
    internal class RestRequestService
    {
        private static readonly ResourceManager ResourceManager = new("WebCardGame.Properties.Resources", typeof(MSGWeb).Assembly);

        public static string GetIndexPage()
        {
            return RestRequestService.ResourceManager.GetString("GameIndex");
        }

        public static bool TryGetFile(string name, out string fileContent)
        {
            fileContent = string.Empty;
            
            string[] splitted = name.Split("/");
            string lastComponent = splitted[^1].Trim().Replace(".json", "").Replace(".js","").Replace(".css","").Replace(".txt", "");
            if (!string.IsNullOrEmpty(lastComponent))
            {
                fileContent = RestRequestService.ResourceManager.GetString(lastComponent);
                return !string.IsNullOrEmpty(fileContent);
            }

            return false;
        }
    }
}
