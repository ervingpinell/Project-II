using Project_II.Models.Dto;
using Project_II.Models.Dao;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using MySql.Data.MySqlClient; // Necesario para conectar con MySQL
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Web.Helpers;



namespace Project_II.Controllers
{
    public class PayoutController : Controller
    {
        private readonly PayoutDao _payoutDao;

        public PayoutController()
        {
            _payoutDao = new PayoutDao();
        }

        // GET: Payout
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payout/Details/5
        public async Task<ActionResult> ShowPayments()
        {
            try
            {
                List<PayoutDto> payments = await _payoutDao.GetPaymentsAsync();
                // Sent the list of payments to the view
                return View(payments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los pagos: " + ex.Message;
                return RedirectToAction("Index", "Contact");
            }
        }


        // GET: Payout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payout/Create 
        [HttpPost]
        public async Task<ActionResult> Create(PayoutDto newPayout)
        {
            try
            {
                // Verify that the model is valid
                if (ModelState.IsValid)
                {
                    var payoutDao = new PayoutDao();
                    newPayout.email = ContactDao.emaill;
                    PayoutDto createdPayout = await payoutDao.CreatePayoutAsync(newPayout);
                    TempData["SuccessMessage"] = "Transaction Done";
                    await _payoutDao.InsertPaymentAsync(createdPayout, newPayout.email);
                    return RedirectToAction("SearchByEmail", "Contact", new { email = ContactDao.emaill });
                }
                else
                {
                    // If the model is not valid, set an error message in TempData
                    TempData["ErrorMessage"] = "No se pudo realizar la transacción, intentelo de nuevo";

                    // Redirect to the previous page
                    return RedirectToAction("SearchByEmail", "Contact", new { email = ContactDao.emaill });
                }
            }
            catch (Exception ex)
            {
                // Handle the error and redirect to the previous page with an error message
                TempData["ErrorMessage"] = "No se pudo realizar la transacción: " + ex.Message;

                // Redirect to the previous page
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}