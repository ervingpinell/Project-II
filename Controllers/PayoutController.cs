﻿using Project_II.Models.Dto;
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
                // Connection DB
                string connectionString = "Server=localhost;Database=payments_db;Uid=root;Pwd=;";
                List<PayoutDto> payments = new List<PayoutDto>();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT id, contact_id, amount, status, created_at, email FROM payments";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            payments.Add(new PayoutDto
                            {
                                id = reader.GetInt32("id"),
                                contact_id = reader.GetInt32("contact_id"),
                                amount = reader.GetDecimal("amount"),
                                status = reader.GetString("status"),
                                created_at = reader.GetDateTime("created_at"),
                                email = reader.GetString("email")
                            });
                        }
                    }

                }

                // Pass the list of payments to the view
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

                    // Logic to save to the database
                    string connectionString = "Server=localhost;Database=payments_db;Uid=root;Pwd=;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        string query = @"INSERT INTO payments (id, contact_id, amount, status, created_at, email) 
                                         VALUES (@id, @contact_id, @amount, @status, @created_at, @email)";

                        using (MySqlCommand command = new MySqlCommand(query, connection))

                        {
                            DateTime dateAdded = DateTime.Now;
                            command.Parameters.AddWithValue("@id", createdPayout.id);
                            command.Parameters.AddWithValue("@contact_id", createdPayout.contact_id);
                            command.Parameters.AddWithValue("@amount", createdPayout.amount);
                            command.Parameters.AddWithValue("@status", createdPayout.status);
                            command.Parameters.AddWithValue("@created_at", dateAdded);
                            command.Parameters.AddWithValue("@email", newPayout.email);

                            await command.ExecuteNonQueryAsync();
                        }
                    }

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