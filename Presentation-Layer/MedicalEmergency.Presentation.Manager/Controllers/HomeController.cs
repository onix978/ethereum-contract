using MedicalEmergency.Domain.Entities;
using MedicalEmergency.Domain.Interfaces.Repositories;
using MedicalEmergency.Infrastructure.Data.Repository;
using MedicalEmergency.Presentation.Manager.Models.Home;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace MedicalEmergency.Presentation.Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHealthUnitRepository _healthUnitRepository;
        public HomeController()
        {
            _healthUnitRepository = new HealthUnitRepository();
        }

        // GET: Home
        public ActionResult Index(string Latitude, string Longitude)
        {
            ViewBag.Latitude = Latitude;
            ViewBag.Longitude = Longitude;

            ViewBag.Flag = 0;

            var list = _healthUnitRepository.GetAll().Select(x => new HealthUnitViewModel { Name = x.Name, Address = x.Address, Latitude = x.Latitude.Replace(",", "."), Longitude = x.Longitude.Replace(",", "."), Phone = x.Phone, ShowRoute = false }).ToList();

            if (Latitude != null && Longitude != null)
            {
                double latitude = ((Math.PI * Convert.ToDouble(Latitude, CultureInfo.InvariantCulture)) / 180);
                double longitude = ((Math.PI * Convert.ToDouble(Longitude, CultureInfo.InvariantCulture)) / 180);

                list = _healthUnitRepository.DitanceReorder(latitude, longitude).Select(x => new HealthUnitViewModel { Name = x.Name, Address = x.Address, Latitude = x.Latitude.Replace(",", "."), Longitude = x.Longitude.Replace(",", "."), Phone = x.Phone, ShowRoute = true, Distance = x.Distance }).ToList();

                return View(list);
            }

            return View(list);
        }
    }
}