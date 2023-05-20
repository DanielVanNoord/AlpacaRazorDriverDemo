using ASCOM.Common.DeviceInterfaces;

namespace AlpacaDriverDemo.Drivers
{
    public class BasicMonitor : ISafetyMonitor
    {
        public bool IsSafe
        {
            get
            {
                if (Connected)
                {
                    return false;
                }
                return true;
            }
        }

        public bool Connected { get; set; } = false;

        public string Description => "A Safety Monitor";

        public string DriverInfo => "A really not functional Safety Monitor";

        public string DriverVersion => "0.1";

        public short InterfaceVersion => 1;

        public string Name => "Safety Monitor";

        public IList<string> SupportedActions => new List<string>();

        public string Action(string ActionName, string ActionParameters)
        {
            throw new ASCOM.NotImplementedException();
        }

        public void CommandBlind(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        public bool CommandBool(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        public string CommandString(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        public void Dispose()
        {
            throw new ASCOM.NotImplementedException();
        }
    }
}
