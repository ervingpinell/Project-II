using Project_II.Models.Dao;
using Project_II.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace Project_II.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactDao _contactDao;

        public ContactController()
        {
            _contactDao = new ContactDao();
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Contact/Details/5
        public async Task<ActionResult> SearchByEmail(string email)
        {
            try
            {
                // Get the contact by email
                var contact = await _contactDao.GetContactByEmail(email);

                if (contact == null)
                {
                    // If the contact is not found, return a NotFound
                    return new HttpStatusCodeResult(404, "Contact not found.");
                }

                // Return the view with the found contact
                return View(contact);
            }
            catch (ArgumentException ex)
            {
                // If the email format is invalid
                return new HttpStatusCodeResult(400, "Bad Request: " + ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                // If there is no valid token
                return new HttpStatusCodeResult(401, "You are not authenticated.");
            }
            catch (Exception ex)
            {
                // Other general errors
                return new HttpStatusCodeResult(500, "An internal error occurred.");
            }
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ContactDto newContact)
        {
            try
            {
                // Verify that the model is valid
                if (ModelState.IsValid)
                {
                    var contactDao = new ContactDao();
                    // Call the CreateContactAsync method of ContactDao to create the new contact
                    ContactDto createdContact = await contactDao.CreateContactAsync(newContact);

                    // If the contact is created successfully, you can redirect or show a success message
                    // Redirect to the contact list or show a success view
                    return RedirectToAction("Index");  // Assuming you have an Index action to list the contacts
                }
                else
                {
                    // If the model is not valid, return the same form with validation errors
                    return View(newContact);
                }
            }
            catch (Exception ex)
            {
                // Handle any error, for example, show an error message in the view
                ModelState.AddModelError("", "Error creating the contact: " + ex.Message);
                return View(newContact);
            }
        }

        // GET: Contact
        public async Task<ActionResult> ShowContacts()
        {
            try
            {
                // Get the list of contacts using the GetContactsAsync method
                List<ContactDto> contacts = await _contactDao.GetContactsAsync();

                // Pass the contacts to the view
                return View(contacts);
            }
            catch (UnauthorizedAccessException)
            {
                // If there is no valid token, redirect to login
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                // If there is an error, show a message or handle the error
                ModelState.AddModelError("", "Error getting the contacts: " + ex.Message);
                return View();
            }
        }

    }
}
