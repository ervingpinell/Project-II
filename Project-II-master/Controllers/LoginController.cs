using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Project_II.Models.Dto;
using Project_II.Models;
using Project_II.Models.Dao;


namespace Project_II.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginDao _loginDao;

        // Constructor
        public LoginController()
        {
            _loginDao = new LoginDao();  // Instantiate the LoginDao directly
        }

        // Login View
        public ActionResult Login()
        {
            return View();
        }

        // Action to handle Login
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Call the AuthenticateUserAsync method of the LoginDao
            var authResult = await _loginDao.AuthenticateUserAsync(model);

            if (authResult != null && !string.IsNullOrEmpty(LoginDao.Token))
            {
                // The token is automatically saved in LoginDao.Token, no need for an additional service.
                // Redirect the user to the main page or dashboard
                return RedirectToAction("Index", "Contact");
            }
            else
            {
                // If authentication fails, show an error message
                ModelState.AddModelError("", "Incorrect username or password.");
                return View(model);
            }
        }

        // Action to validate the session (example of protecting an endpoint)
        public async Task<ActionResult> ValidateSession()
        {
            // Get the current token from the static property of LoginDao
            var token = LoginDao.Token;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login"); // If there is no token, redirect to login
            }

            // Check if the token is still active by calling the IsTokenValidAsync method of LoginDao
            bool isTokenValid = await _loginDao.IsTokenValidAsync();

            if (isTokenValid)
            {
                return RedirectToAction("Index", "Contact");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }

}
