using ASCOM.Alpaca;

namespace AlpacaDriverDemo
{
    internal class AlpacaConfiguration : IAlpacaConfiguration
    {
        public bool RunInStrictAlpacaMode => true;

        public bool PreventRemoteDisconnects => true;

        public string ServerName => "Alpaca Demo";

        public string Manufacturer => "Daniel";

        public string ServerVersion => "0";

        public string Location => "My House";

        public bool AllowImageBytesDownload => true;

        public bool AllowDiscovery => true;

        public int ServerPort => 5077;

        public bool AllowRemoteAccess => true;

        public bool LocalRespondOnlyToLocalHost => true;

        public bool RunSwagger => true;
    }
}