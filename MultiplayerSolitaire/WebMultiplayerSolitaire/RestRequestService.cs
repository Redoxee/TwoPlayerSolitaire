using Microsoft.AspNetCore.Http;
using System.Resources;

namespace WebMultiplayerSolitaire
{
    public class RestRequestService
    {
        private static ResourceManager ResourceManager = new ResourceManager("WebCardGame.Properties.Resources", typeof(Program).Assembly);

        public static string HandleRestRequest(HttpContext context)
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
            string lastComponent = splitted[splitted.Length - 1].Trim().Replace(".js","");
            if (!string.IsNullOrEmpty(lastComponent))
            {
                fileContent = RestRequestService.ResourceManager.GetString(lastComponent);
                return !string.IsNullOrEmpty(fileContent);
            }

            return false;
        }
    }
}
