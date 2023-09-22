namespace PriceCheck.DB
{
    public class SecretsFile
    {
        public const string DataDotGovKey = "api.Key.Gov";
        public const string DbPasswordKey = "db.Password.ManyMouths";

        /// <summary>
        /// This should be a file adjacent to us
        /// </summary>
        public string FilePath = "./.env";

        public SecretsFile()
        {
        }

        public Dictionary<string, string> KVP { get; } = new();

        public string DatabasePassword()
        {
            return GetSecret(DbPasswordKey);
        }

        /// <summary>
        /// Loads our api key from the configuration file that contains it.
        /// If the config has already been loaded, return the existing API key
        /// </summary>
        /// <returns></returns>
        public string DataDotGovApiKey()
        {
            return GetSecret(DataDotGovKey);
        }

        public string GetSecret(string key)
        {
            if (!WasInit())
            {
                Initialize();
            }

            if (!KVP.ContainsKey(key))
            {
                throw new Exception($"Cannot access secret, API key not found. [Key: {key}]");
            }

            return KVP[key];
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