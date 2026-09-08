using ASCOM.Common.DeviceInterfaces;
using System.ComponentModel.DataAnnotations;

namespace AlpacaDriverDemo.DeviceAccess
{
    /// <summary>
    /// This class implements the ISafetyMonitorV3 interface and provides basic functionality for connecting, disconnecting, and monitoring the safety state of the device.
    /// </summary>
    public class BasicMonitor : ISafetyMonitorV3
    {

        #region Connect, Disconnect, Connecting and Connected members

        // Private constants associated with connection and disconnection
        private const int CONNECTION_ERROR_NUMBER = unchecked((int)0x80040500); // Error number for connection errors. This is outside the ASCOM reserved range of 0x80040400 to 0x800404FF.

        // Private variables associated with connection and disconnection
        private bool _connected = false; // Current state of the Connected property.
        private bool _connecting = false; // Current state of the Connecting property.
        private Exception? _lastConnectionException = null; // The last exception thrown during a Connect() or Disconnect() operation (reset when a new Connect or Disconnect operation is started)
        private readonly object _connectionLock = new object(); // Lock object to ensure that only one connection or disconnection operation can be in progress at any time.

        /// <summary>
        /// Gets or sets a value indicating whether the device is connected to the hardware (operates synchronously).
        /// </summary>
        public bool Connected
        {
            get => _connected;
            set
            {
                if (value != _connected) // Device is not in the requested state so change it to the requested state.
                {
                    // Connect or disconnect the device
                    if (value) // Connect the device
                    {
                        // Call the Connect method. If it fails the exception will propagate up to the client.
                        Connect();
                    }
                    else // Disconnect the device
                    {
                        // Call the Disconnect method. If it fails the exception will propagate up to the client.
                        Disconnect();
                    }

                    // Wait for the operation to complete (Connecting = false) or fail. If the operation fails and Connecting throws, the exception will propagate up to the client.
                    while (Connecting)
                    {
                        // Poll every 250ms and wait for the operation to complete or fail.
                        Thread.Sleep(250);
                    }
                }
                else // Device is already in the requested state.
                {
                    // Included for completeness - No action required because the value is already set to the requested value.
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the device is in the process of asynchronously connecting or disconnecting, initiated by the Connect or Disconnect methods.
        /// </summary>
        /// <exception cref="ASCOM.DriverException">Thrown if there was a connection error during the last <see cref="Connect"/> or <see cref="Disconnect"/> attempt.</exception>
        public bool Connecting
        {
            get
            {
                // Check whether there was a connection error during the last connection attempt.
                if (_lastConnectionException is not null) // There was a connection error so throw an exception containing details of the error.
                {
                    throw new ASCOM.DriverException($"A connection error occurred - {_lastConnectionException.Message}", CONNECTION_ERROR_NUMBER, _lastConnectionException);
                }

                // No connection error so return the current connecting state.
                return _connecting;
            }
            set
            {
                _connecting = value;
            }
        }

        /// <summary>
        /// Starts an asynchronous connection to the device.
        /// </summary>
        public void Connect()
        {
            lock (_connectionLock) // Ensure that only one connection or disconnection operation can be in progress at any time.
            {
                // Make sure we are currently disconnected and are not trying to connect or disconnect
                if (!Connected && !IsConnecting()) // Start the connection process because we are neither connected nor trying to connect / disconnect.
                {
                    // Create a task to effect the connection to the device. This is done in a task to avoid blocking the calling thread.
                    Task connectTask = new Task(() =>
                    {
                        try
                        {
                            // Clear any previous connection / disconnection error before attempting to connect.
                            _lastConnectionException = null;

                            //
                            // TODO: Add your device connection code here.
                            //

                            // Set Connected to true before setting Connecting false indicating that the operation has completed successfully.
                            Connected = true;
                            Connecting = false;
                        }
                        catch (Exception ex)
                        {
                            // Set Connecting false to indicate that the operation has completed, albeit with an error. Connected will retain its pre-operation state.
                            Connecting = false;

                            // Save the exception so that it can be returned through the Connecting property.
                            _lastConnectionException = ex;

                            // Log the error
                            Console.WriteLine($"An error occurred while connecting - {ex.Message}.\r\n{ex}");
                        }
                    });

                    // Set connecting true before starting the task to avoid race conditions
                    Connecting = true;

                    // Start the connection task
                    connectTask.Start();
                }
                else // Already connected or trying to connect.
                {
                    // Included for completeness - No action required because we are already connected or trying to connect.
                }
            }
        }

        /// <summary>
        /// Starts an asynchronous disconnection from the device.
        /// </summary>
        public void Disconnect()
        {
            lock (_connectionLock) // Ensure that only one connection or disconnection operation can be in progress at any time.
            {
                // Make sure we are currently connected and are not trying to connect or disconnect
                if (Connected && !IsConnecting()) // Start the disconnection process because we are neither disconnected nor trying to connect / disconnect.
                {
                    // Create a task to effect the disconnection from the device. This is done in a task to avoid blocking the calling thread.
                    Task disconnectTask = new Task(() =>
                    {
                        try
                        {
                            // Clear any previous connection / disconnection error before attempting to disconnect.
                            _lastConnectionException = null;

                            //
                            // TODO: Add your device disconnection code here.
                            //

                            // Set Connected to false before setting Connecting false indicating that the operation has completed successfully.
                            Connected = false;
                            Connecting = false;
                        }
                        catch (Exception ex)
                        {
                            // Set Connecting false to indicate that the operation has completed, albeit with an error. Connected will retain its pre-operation state.
                            Connecting = false;

                            // Save the exception so that it can be returned through the Connecting property.
                            _lastConnectionException = ex;

                            // Log the error
                            Console.WriteLine($"An error occurred while disconnecting - {ex.Message}.\r\n{ex}");
                        }
                    });

                    // Set connecting true before starting the task to avoid race conditions
                    Connecting = true;

                    // Start the connection task
                    disconnectTask.Start();
                }
                else // Already disconnected or trying to disconnect.
                {
                    // Included for completeness - No action required because we are already disconnected or trying to disconnect.
                }
            }
        }

        /// <summary>
        /// Return an absolute true / false state for Connecting, interpreting exceptions to mean that Connecting is false.
        /// </summary>
        /// <returns></returns>
        private bool IsConnecting()
        {
            // Check whether there was a connection error during the last connection attempt.
            if (_lastConnectionException is not null) // There was a connection error.
                // Return false because the last attempt failed and there is no Connect or Disconnect in progress
                return false;

            // No connection error so return the current connecting state
            return _connecting;
        }

        #endregion

        #region Members common to all interfaces

        /// <summary>
        /// Device description string. This may be displayed to the user in a user interface.
        /// </summary>
        public string Description => "A Safety Monitor";

        /// <summary>
        /// Driver information string. This may be displayed to the user in a user interface.
        /// </summary>
        public string DriverInfo => "A really not functional Safety Monitor";

        /// <summary>
        /// Driver version string. This may be displayed to the user in a user interface.
        /// </summary>
        public string DriverVersion => "0.1";

        /// <summary>
        /// Interface version number. This may be displayed to the user in a user interface.
        /// </summary>
        public short InterfaceVersion => 3;

        /// <summary>
        /// Device name string.
        /// </summary>
        public string Name => "Safety Monitor";

        /// <summary>
        /// Gets a list of the device-specific actions supported by this driver. The list is empty if no device-specific actions are supported.
        /// </summary>
        public IList<string> SupportedActions => new List<string>();

        /// <summary>
        /// Gets a list of the state values supported by this device.
        /// </summary>
        /// <remarks>See https://ascom-standards.org/newdocs for more information on which state values should be returned by your particular device type.</remarks>
        public List<StateValue> DeviceState => new List<StateValue>()
        {
            new StateValue(nameof(IsSafe), IsSafe.ToString()), // Safety monitor specific state value.
            new StateValue("TimeStamp", DateTime.UtcNow.ToString()) // Common to all device interfaces.
        };

        /// <summary>
        /// Executes a device-specific action.
        /// </summary>
        /// <param name="ActionName">The name of the action to execute.</param>
        /// <param name="ActionParameters">The parameters for the action.</param>
        /// <returns>The result of the action.</returns>
        public string Action(string ActionName, string ActionParameters)
        {
            throw new ASCOM.NotImplementedException();
        }

        /// <summary>
        /// Sends a command to the device without expecting a response.
        /// </summary>
        /// <param name="Command">The command to send.</param>
        /// <param name="Raw">Indicates whether the command is raw.</param>
        public void CommandBlind(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        /// <summary>
        /// Sends a command to the device and returns a boolean response.
        /// </summary>
        /// <param name="Command">The command to send.</param>
        /// <param name="Raw">Indicates whether the command is raw.</param>
        /// <returns>The boolean response from the device.</returns>
        public bool CommandBool(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        /// <summary>
        /// Sends a command to the device and returns a string response.
        /// </summary>
        /// <param name="Command">The command to send.</param>
        /// <param name="Raw">Indicates whether the command is raw.</param>
        /// <returns>The string response from the device.</returns>
        public string CommandString(string Command, bool Raw = false)
        {
            throw new ASCOM.NotImplementedException();
        }

        /// <summary>
        /// Disposes of the resources used by the device.
        /// </summary>
        public void Dispose()
        {
            throw new ASCOM.NotImplementedException();
        }

        #endregion

        #region Safety monitor specific members

        /// <summary>
        /// Gets a value indicating whether the device is in a safe state.
        /// </summary>
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

        #endregion

    }
}
