using Microsoft.AspNetCore.Mvc;
using PTC.Data;
using PTC.Models;
using PTC.Services;
using PTC.Utils;

namespace PTC.Controllers
{

    public class MailController(IEmailSender emailSender, PtcContext context) : ControllerBase
    {
        private readonly PtcContext db = context;
        private readonly IEmailSender _emailSender = emailSender;
        private string sendto;
        private string? _cc1;
        private string? _cc2;

        public async Task<IActionResult> Index(GuestReg guest, DdedmEmployee met, DdedmEmployee req)
        {
            var subject = "Guest Registration";
            var path = Directory.GetCurrentDirectory();
            var template = await System.IO.File.ReadAllTextAsync(path + "\\wwwroot\\Template\\index.html");
            var qrCodeUrl = QrCodeGenerator.GenerateQrCode(guest.Id.ToString());

            var startDate = guest.StartDate.ToString("yyyy-MM-dd");
            var endDate = guest.EndDate.ToString("yyyy-MM-dd");


            var getMet = await EmployeeService.GetEmployeeByEmpId(guest.MetId, db);
            string getMetName = getMet.Name.ToString();

            var getReq = await EmployeeService.GetEmployeeByEmpId(guest.ReqId, db);
            string getReqName = getReq.Name.ToString();

            var companyType = guest.CompanyType.ToString() == "Non Panasonic" ? "No" : "Yes";

            var overseas = guest.CountryCode == "IDN" ? "No" : "Yes";

            SetEmails(guest.Email, req, met);

            var replacementValues = new Dictionary<string, string?>
            {
                {"{GuestStartDate}", startDate},
                {"{GuestEndDate}", endDate},
                {"{GuestName}", guest.Name},
                {"{GuestCompany}", guest.Company},
                {"{GuestCompanyType}", companyType},
                {"{GuestDepartment}", guest.DeptSect},
                {"{GuestNumber}", guest.Total.ToString()},
                {"{GuestPurpose}", guest.Requirement},
                {"{GuestEmail}", guest.Email},
                {"{GuestIdentificationNumber}", guest.NationalId},
                {"{GuestRequester}", getReqName},
                {"{GuestMet}", getMetName},
                {"{GuestCategory}", await GuestService.GetCategoryName(guest.CategoryId, db)},
                {"{GuestOverseas}", overseas },
                {"{GuestCountry}", await GuestService.GetCountryName(guest.CountryCode, db) }
            };

            var message = ReplacePlaceholders(template, replacementValues);

            try
            {
                await _emailSender.SendEmailGuest(sendto, _cc1, _cc2, subject, message, qrCodeUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex}");
                throw;
            }
        }

        private void SetEmails(string? guest, DdedmEmployee req, DdedmEmployee met)
        {
            if (guest != null)
            {
                sendto = guest;
                if (req == met)
                {
                    _cc1 = req.Email ?? req.PrivateEmail;
                    _cc2 = null;
                }
                else
                {
                    _cc1 = req.Email ?? req.PrivateEmail;
                    _cc2 = met.Email ?? met.PrivateEmail;
                }
            }
            else if (!string.IsNullOrEmpty(req.Email) || !string.IsNullOrEmpty(req.PrivateEmail))
            {
                sendto = req.Email ?? req.PrivateEmail;
                if (req == met)
                {
                    _cc1 = null;
                    _cc2 = null;
                }
                else
                {
                    _cc1 = met.Email ?? met.PrivateEmail;
                    _cc2 = null;
                }
            }
            else
            {
                sendto = met.Email ?? met.PrivateEmail;
                _cc1 = null;
                _cc2 = null;
            }
        }


        private static string ReplacePlaceholders(string template, Dictionary<string, string?> replacements)
        {
            foreach (var replacement in replacements)
            {
                template = template.Replace(replacement.Key, replacement.Value);
            }

            return template;
        }
    }
}