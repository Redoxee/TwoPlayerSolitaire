using Microsoft.AspNetCore.Http;
using System.Resources;

namespace WebMultiplayerSolitaire
{
    public class RestRequestService
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("WebCardGame.Properties.Resources", typeof(Program).Assembly);

        public static string HandleRestRequest()
        {
            return GetIndexPage();
        }

        public static string GetIndexPage()
        {
            return RestRequestService.ResourceManager.GetString("GameIndex");
        }

        public static bool TryGetFile(string name, out string fileContent)
        {
            fileContent = string.Empty;
            
            string[] splitted = name.Split("/");
            string lastComponent = splitted[^1].Trim().Replace(".json", "").Replace(".js","").Replace(".css","");
            if (!string.IsNullOrEmpty(lastComponent))
            {
                fileContent = RestRequestService.ResourceManager.GetString(lastComponent);
                return !string.IsNullOrEmpty(fileContent);
            }

            return false;
        }
    }
}
