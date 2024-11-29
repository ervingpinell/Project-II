using Project_II.Models.Dto;
using Project_II.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace Project_II.Controllers
{
    public class PayoutController : Controller
    {

        private readonly PayoutDao _payoutDao;

        public PayoutController()
        {
            _payoutDao = new PayoutDao();
        }

        // GET: Payouit
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payouit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Payouit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payouit/Create
        [HttpPost]
        public async Task<ActionResult> Create(PayoutDto newPayout)
        {
            try
            {
                // Verify that the model is valid
                if (ModelState.IsValid)
                {
                    var payoutDao = new PayoutDao();
                    // Call the CreateContactAsync method of ContactDao to create the new contact
                    PayoutDto createdPayout = await payoutDao.CreatePayoutAsync(newPayout);

                    // If the contact is created successfully, you can redirect or show a success message
                    // Redirect to the contact list or show a success view
                    return RedirectToAction("Index");  // Assuming you have an Index action to list the contacts
                }
                else
                {
                    // If the model is not valid, return the same form with validation errors
                    return View(newPayout);
                }
            }
            catch (Exception ex)
            {
                // Handle any error, for example, show an error message in the view
                ModelState.AddModelError("", "Error creating the contact: " + ex.Message);
                return View(newPayout);
            }
        }
    }
}

