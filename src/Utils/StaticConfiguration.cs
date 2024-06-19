using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aeternaCaptions.src.Utils
{
    public static class StaticConfiguration
    {
        public static IConfiguration? config;
        public static string? AssemblyAiApiKey;
        public static string? DeepLApiKey;
        public static void Initialize(string AAIApiKey, string DLApiKey, IConfiguration Configuration)
        {
            config = Configuration;
            AssemblyAiApiKey = AAIApiKey;
            DeepLApiKey = DLApiKey;
        }
        
    }
}