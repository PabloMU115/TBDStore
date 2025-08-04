using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;
using TBD.Data;
using TBD.Models.ViewModels;


namespace TBD.Models.ViewComponents
{
    public class PasarelaDePagoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private string PayPalClientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        private string PayPalUrl { get; set; } = "";

        public PasarelaDePagoViewComponent(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            PayPalClientId = configuration["PayPalSettings:ClientId"] ?? throw new ArgumentNullException("ClientId");
            PayPalSecret = configuration["PayPalSettings:Secret"] ?? throw new ArgumentNullException("Secret");
            PayPalUrl = configuration["PayPalSettings:Url"] ?? throw new ArgumentNullException("Url");
        }

        public IViewComponentResult Invoke()
        {
            return View(model: PayPalClientId);
        }
    }
}
