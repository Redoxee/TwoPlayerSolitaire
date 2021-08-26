﻿using Microsoft.AspNetCore.Http;
using System.Resources;

namespace WebMultiplayerSolitaire
{
    public class RestRequestService
    {
        public static string HandleRestRequest(HttpContext context)
        {
            return GetIndexPage();
        }

        public static string GetIndexPage()
        {
            ResourceManager resourceManager = new ResourceManager("WebCardGame.Properties.Resources", typeof(Program).Assembly);
            string indexFile = resourceManager.GetString("GameIndex");

            string insert = string.Empty;
            string response = indexFile;

            return response;
        }
    }
}
