using Microsoft.AspNetCore.Mvc;

namespace PTC.Controllers
{
    public class RouteController : Controller
    {
        public IActionResult Home()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult GuestRegist()
        {
            return RedirectToAction("Index", "GuestRegist");
        }
        public IActionResult ShowGuestPlan()
        {
            return RedirectToAction("Index", "ShowGuestPlan");
        }
        public IActionResult ShowGuestTraffic() 
        {
            return RedirectToAction("Index", "ShowGuestTraffic");
        }
        public IActionResult EmployeeIn()
        {
            return RedirectToAction("Index", "EmployeeIn");
        }
        public IActionResult ShowWorkerPlan()
        {
            return RedirectToAction("Index", "ShowWorkerPlan");
        }
        public IActionResult ShowWorkerTraffic()
        {
            return RedirectToAction("Index", "ShowWorkerTraffic");
        }
        public IActionResult ShowEmpTraffic()
        {
            return RedirectToAction("Index", "ShowEmpTraffic");
        }
        public IActionResult ShowEmpPlan()
        {
            return RedirectToAction("Index", "ShowEmpPlan");
        }
        public IActionResult ShowSupplierTraffic()
        {
            return RedirectToAction("index", "ShowSupplierTraffic");
        }
        public IActionResult AddUser()
        {
            return RedirectToAction("Index", "AddUser");
        }
        public IActionResult AddMobileUser()
        {
            return RedirectToAction("Index", "AddMobileUser");
        }
        public IActionResult AddGuestCategory() 
        {

            return RedirectToAction("Index", "AddGuestCategory");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            string ipAddress = "10.83.34.70";
            int portNumber = 8080;

            return Redirect($"http://{ipAddress}:{portNumber}");
        }
    }
}