using Newtonsoft.Json;
using Project_II.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace Project_II.Models.Dao
{

    public class ContactDao
    {
        public static string emaill;
        private readonly string apiBaseUrl = "https://saacapps.com/payout/contact.php"; // URL of the contacts API

        // Method to validate if the email address has the correct format
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";  // Basic regular expression for email
            return Regex.IsMatch(email, emailPattern);
        }

        // Method to create a new contact
        public async Task<ContactDto> CreateContactAsync(ContactDto newContact)
        {
            // Check that the newContact object is not null
            if (newContact == null)
            {
                throw new ArgumentNullException(nameof(newContact), "The contact cannot be null.");
            }

            // Create HTTP client
            var client = new HttpClient();

            // Check if there is a token available
            string token = LoginDao.Token;

            if (string.IsNullOrEmpty(token))
            {
                // If there is no token, throw an exception or handle the error
                throw new UnauthorizedAccessException("No valid token found.");
            }

            // URL of the contact creation endpoint
            string requestUrl = "https://saacapps.com/payout/contact.php";  // Adjust this URL according to your actual endpoint

            // Convert the newContact object to JSON
            string jsonContent = JsonConvert.SerializeObject(newContact);

            // Create the HTTP request content (in JSON format)
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Create the HTTP request message with POST method
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            // Add the Authorization header with the token
            request.Headers.Add("Authorization", $"Bearer {token}");

            // Perform the POST request to create the new contact
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a ContactDto object
                var result = JsonConvert.DeserializeObject<ContactDto>(await response.Content.ReadAsStringAsync());
                return result;
            }
            else
            {
                // Capture the error content to diagnose the problem
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating the contact: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
            }
        }

        // Method to get a contact by email
        public async Task<ContactDto> GetContactByEmail(string email)
        {


            // Validate that the email address is in a correct format
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("The email address format is not valid.");
            }

            var client = new HttpClient();
            emaill = email;
            // Check if there is a token available
            string token = LoginDao.Token;

            if (string.IsNullOrEmpty(token))
            {
                // If there is no token, throw an exception or handle the error
                throw new UnauthorizedAccessException("No valid token found.");
            }

            // Set the Authorization header with the token
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Create the URL with the email parameter

            string requestUrl = $"{apiBaseUrl}?email={HttpUtility.UrlEncode(email)}"; // Escape the email

            // Perform the GET request to get the contact
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a ContactDto object
                var result = JsonConvert.DeserializeObject<ContactDto>(await response.Content.ReadAsStringAsync());
                return result;
            }
            else
            {
                // If the response is not successful, throw an exception with more details
                var errorContent = await response.Content.ReadAsStringAsync(); // Capture the error content
                throw new Exception($"Error getting the contact: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
            }
        }

        // Method to get the list of contacts (without modifications)
        public async Task<List<ContactDto>> GetContactsAsync()
        {
            var client = new HttpClient();

            // Check if there is a token available
            string token = LoginDao.Token;

            if (string.IsNullOrEmpty(token))
            {
                // If there is no token, throw an exception or handle the error
                throw new UnauthorizedAccessException("No valid token found.");
            }

            // Set the Authorization header with the token
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Perform the GET request to get the contacts
            var response = await client.GetAsync(apiBaseUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a list of ContactDto objects
                var result = JsonConvert.DeserializeObject<List<ContactDto>>(await response.Content.ReadAsStringAsync());
                return result;
            }

            // If the response is not successful, throw an exception with more details
            var errorContent = await response.Content.ReadAsStringAsync(); // Capture the error content
            throw new Exception($"Error getting the contacts: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
        }
    }
}
