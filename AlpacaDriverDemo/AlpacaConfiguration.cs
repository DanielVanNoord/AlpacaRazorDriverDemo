using ASCOM.Alpaca;

namespace AlpacaDriverDemo
{
    internal class AlpacaConfiguration : IAlpacaConfiguration
    {
        public bool RunInStrictAlpacaMode => ServerSettings.RunInStrictAlpacaMode;

        public bool PreventRemoteDisconnects => ServerSettings.PreventRemoteDisconnects;

        public string ServerName => Program.ServerName;

        public string Manufacturer => Program.Manufacturer;

        public string ServerVersion => Program.ServerVersion;

        public string Location => ServerSettings.Location;

        public bool AllowImageBytesDownload => ServerSettings.AllowImageBytesDownload;

        public bool AllowDiscovery => ServerSettings.AllowDiscovery;

        public int ServerPort => ServerSettings.ServerPort;

        public bool AllowRemoteAccess => ServerSettings.AllowRemoteAccess;

        public bool LocalRespondOnlyToLocalHost => ServerSettings.LocalRespondOnlyToLocalHost;

        public bool RunSwagger => ServerSettings.RunSwagger;
    }
}