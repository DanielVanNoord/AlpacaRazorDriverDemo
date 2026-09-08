using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace AlpacaDriverDemo.Pages
{
    /// <summary>
    /// Interaction logic for the Error page.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Gets or sets the request ID for the current request.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the request ID.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
        /// <summary>
        /// The logger instance used to log messages for this page.
        /// </summary>
        private readonly ILogger<ErrorModel> _logger;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to use for logging messages.</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Handles GET requests to the Error page.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}