using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AlpacaDriverDemo
{
    internal static class ServerSettings
    {
        //This is a shared profile that is used to store server settings.
        internal static ASCOM.Tools.XMLProfile Profile = new ASCOM.Tools.XMLProfile(Program.DriverID, "Server");

        internal static string Location
        {
            get
            {
                return Profile.GetValue("Location", "Unknown");
            }
            set
            {
                Profile.WriteValue("Location", value.ToString());
            }
        }

        internal static bool AutoStartBrowser
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AutoStartBrowser", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("AutoStartBrowser", value.ToString());
            }
        }

        internal static ushort ServerPort
        {
            get
            {
                if (ushort.TryParse(Profile.GetValue("ServerPort", Program.DefaultPort.ToString()), out ushort result))
                {
                    return result;
                }
                return Program.DefaultPort;
            }
            set
            {
                Profile.WriteValue("ServerPort", value.ToString());
            }
        }

        internal static bool AllowRemoteAccess
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AllowRemoteAccess", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("AllowRemoteAccess", value.ToString());
            }
        }

        internal static bool AllowDiscovery
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("AllowDiscovery", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("AllowDiscovery", value.ToString());
            }
        }

        internal static bool LocalRespondOnlyToLocalHost
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("LocalRespondOnlyToLocalHost", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("LocalRespondOnlyToLocalHost", value.ToString());
            }
        }

        internal static bool PreventRemoteDisconnects
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("PreventRemoteDisconnects", false.ToString()), out bool result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                Profile.WriteValue("PreventRemoteDisconnects", value.ToString());
            }
        }

        internal static bool RunSwagger
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("RunSwagger", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("RunSwagger", value.ToString());
            }
        }

        internal static bool AllowImageBytesDownload
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("CanImageBytesDownload", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("CanImageBytesDownload", value.ToString());
            }
        }

        internal static bool RunInStrictAlpacaMode
        {
            get
            {
                if (bool.TryParse(Profile.GetValue("RunInStrictAlpacaMode", true.ToString()), out bool result))
                {
                    return result;
                }
                return true;
            }
            set
            {
                Profile.WriteValue("RunInStrictAlpacaMode", value.ToString());
            }
        }
    }
}