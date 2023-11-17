using ASCOM.Alpaca;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

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

            var builder = WebApplication.CreateBuilder(args);

            //For Debug ConsoleLogger is very nice. For production TraceLogger is recommended.
            ASCOM.Tools.ConsoleLogger Logger = new ASCOM.Tools.ConsoleLogger();

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