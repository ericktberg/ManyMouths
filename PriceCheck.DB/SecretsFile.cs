namespace PriceCheck.DB
{
    public class SecretsFile
    {
        /// <summary>
        /// This should be a file adjacent to us
        /// </summary>
        public string FilePath = "./.env";

        public SecretsFile()
        {
        }

        public Dictionary<string, string> KVP { get; } = new();

        /// <summary>
        /// Loads our api key from the configuration file that contains it.
        /// If the config has already been loaded, return the existing API key
        /// </summary>
        /// <returns></returns>
        public string DataDotGovApiKey()
        {
            if (!WasInit())
            {
                Initialize();
            }

            string dataDotGovKeyname = "DotGovApiKey";
            if (!KVP.ContainsKey(dataDotGovKeyname))
            {
                throw new Exception("Cannot access data dot gov, API key not found.");
            }

            return KVP[dataDotGovKeyname];
        }

        public void Initialize()
        {
            if (!File.Exists(FilePath))
            {
                // What to do??
                throw new Exception("Secrets file not found! Cannot load API keys");
            }

            string text = File.ReadAllText(FilePath);
            var lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var line in lines)
            {
                var kvp = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (kvp.Length != 2)
                {
                    continue;
                }

                if (KVP.ContainsKey(kvp[0]))
                {
                    continue;
                }

                KVP.Add(kvp[0], kvp[1]);
            }
        }

        private bool WasInit()
        {
            return KVP.Any();
        }
    }
}