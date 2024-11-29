using Project_II.Models.Dto;
using Project_II.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
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

                return View();
            
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
                // Verificar que el modelo es válido
                if (ModelState.IsValid)
                {
                    var payoutDao = new PayoutDao();
                    newPayout.email = ContactDao.emaill;
                    PayoutDto createdPayout = await payoutDao.CreatePayoutAsync(newPayout);
                    TempData["SuccessMessage"] = "Transaction Done";
             
                    return RedirectToAction("SearchByEmail", "Contact", new { email = ContactDao.emaill });
                }

                else
                {

                    // Si el modelo no es válido, establecer un mensaje de error en TempData
                    TempData["ErrorMessage"] = "No se pudo realizar la transacción, intentelo de nuevo";

                    // Redirigir a la página anterior
                    return RedirectToAction("SearchByEmail", "Contact", new {email = ContactDao.emaill});
                }
            }

            catch (Exception ex)
            {
                // Manejar el error y redirigir a la página anterior con un mensaje de error
                TempData["ErrorMessage"] = "No se pudo realizar la transacción: " + ex.Message;

                // Redirigir a la página anterior
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}

