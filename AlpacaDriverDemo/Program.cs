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
        internal const int DefaultPort = 1234;

        //Fill these out
        internal const string Manufacturer = "Your name here";
        internal const string ServerName = "A friendly name for the server";
        internal const string ServerVersion = "1.0";


        public static void Main(string[] args)
        {
            //First fill in information for your driver in the Alpaca Configuration Class. Some of these you may want to store in a user changeable settings file.

            var builder = WebApplication.CreateBuilder(args);

            ASCOM.Tools.ConsoleLogger Logger = new ASCOM.Tools.ConsoleLogger();

            //Attach the logger
            ASCOM.Alpaca.Logging.AttachLogger(Logger);
            ASCOM.Alpaca.DeviceManager.LoadConfiguration(new AlpacaConfiguration());

            ASCOM.Alpaca.DeviceManager.LoadSafetyMonitor(0, new Drivers.BasicMonitor(), "Really Basic Safety", "321654562163");

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