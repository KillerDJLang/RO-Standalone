using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Aki.Common.Http;

namespace DJsROStandalone.Helpers
{
    public static class Utils
    {
        public static readonly List<string> Traders = new List<string>
        {
            "54cb50c76803fa8b248b4571",     //Prapor
            "54cb57776803fa99248b456e",     //Therapist 
            "579dc571d53a0658a154fbec",     //Fence
            "58330581ace78e27b8b10cee",     //Skier 
            "5935c25fb3acc3127c3d8cd9",     //Peacekeeper 
            "5a7c2eca46aef81a7ca2145d",     //Mechanic 
            "5ac3b934156ae10c4430e83c",     //Ragman 
            "5c0647fdd443bc2504c2d371"      //Jaeger
        };

        public static T Get<T>(string url)
        {
            var req = RequestHandler.GetJson(url);

            if (string.IsNullOrEmpty(req))
            {
                throw new InvalidOperationException("The response from the server is null or empty.");
            }

            return JsonConvert.DeserializeObject<T>(req);
        }
    }
}
