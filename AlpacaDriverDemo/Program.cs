using ASCOM.Alpaca;
using ASCOM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AlpacaDriverDemo
{
    public class Program
    {
        //Fill this with your driver name
        internal const string DriverID = "Alpaca.CustomDevice";

        //Change this to a unique value
        //You should offer a way for the end user to customize this via the command line so it can be changed in the case of a collision.
        internal const int DefaultPort = 1234;

        //Fill these out
        internal const string Manufacturer = "Your name here";
        internal const string ServerName = "A friendly name for the server";
        internal const string ServerVersion = "1.0";


        public static void Main(string[] args)
        {
            //First fill in information for your driver in the Alpaca Configuration Class. Some of these you may want to store in a user changeable settings file.

            //For Debug ConsoleLogger is very nice. For production TraceLogger is recommended.
            ASCOM.Tools.ConsoleLogger Logger = new ASCOM.Tools.ConsoleLogger();

            Logger.LogInformation($"{ServerName} version {ServerVersion}");
            Logger.LogInformation($"Running on: {RuntimeInformation.OSDescription}.");

            //Reset all stored settings if requested
            if (args?.Any(str => str.Contains("--reset")) ?? false)
            {
                Logger.LogInformation("Reseting Settings");
                ServerSettings.Reset();

                //If you have any device settings you should reset them as well or add a specific reset command.

                return;
            }


            //Turn off Authentication. Once off the user can change the password and re-enable authentication
            if (args?.Any(str => str.Contains("--reset-auth")) ?? false)
            {
                Logger.LogInformation("Turning off Authentication to allow password reset.");
                ServerSettings.UseAuth = false;
                Logger.LogInformation("Authentication off, you can change the password and then re-enable Authentication.");
            }

            if (args?.Any(str => str.Contains("--local-address")) ?? false)
            {
                Console.WriteLine($"http://localhost:{ServerSettings.ServerPort}");
            }

            if (!args?.Any(str => str.Contains("--urls")) ?? true)
            {
                if (args == null)
                {
                    args = new string[0];
                }

                Logger.LogInformation("No startup url args detected, binding to saved server settings.");

                var temparray = new string[args.Length + 1];

                args.CopyTo(temparray, 0);

                string startupURLArg = "--urls=http://";

                //If set to allow remote access bind to all local ips, otherwise bind only to localhost
                if (ServerSettings.AllowRemoteAccess)
                {
                    startupURLArg += "*";
                }
                else
                {
                    startupURLArg += "localhost";
                }

                startupURLArg += ":" + ServerSettings.ServerPort;

                Logger.LogInformation("Startup URL args: " + startupURLArg);

                temparray[args.Length] = startupURLArg;

                args = temparray;
            }

            var builder = WebApplication.CreateBuilder(args);

            //Attach the logger
            ASCOM.Alpaca.Logging.AttachLogger(Logger);
            ASCOM.Alpaca.DeviceManager.LoadConfiguration(new AlpacaConfiguration());

            //Add a safety monitor with device id 0. You can load any number of the same device with different ids or load other devices with Load* functions. 
            //You may want to inject settings and logging here to the Driver Instance.
            //For each device you add you should add a setting page to the settings folder and an entry in the Shared NavMenu
            ASCOM.Alpaca.DeviceManager.LoadSafetyMonitor(0, new Drivers.BasicMonitor(), "Really Basic Safety Monitor", ServerSettings.GetDeviceUniqueId("SafetyMonitor", 0));

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            //Load any xml comments for this program, this helps with swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //Add Swagger for the APIs
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureSwagger(builder.Services, xmlPath);
            //Set default behaviors for Alpaca APIs
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureAlpacaAPIBehavoir(builder.Services);
            //Use Authentication
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureAuthentication(builder.Services);
            //Add User Service
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            //Start Swagger on the Swagger endpoints if enabled.
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureSwagger(app);

            //Configure Discovery
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureDiscovery(app);

            //Allow authentication, either Cookie or Basic HTTP Auth
            ASCOM.Alpaca.Razor.StartupHelpers.ConfigureAuthentication(app);

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();

            app.MapControllers();

            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}