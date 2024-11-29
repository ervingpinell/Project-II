using Newtonsoft.Json;
using Project_II.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project_II.Models.Dao
{
    public class PayoutDao
    {


        public async Task<PayoutDto> CreatePayoutAsync(PayoutDto newPayout)
        {
            // Check that the newContact object is not null
            if (newPayout == null)
            {
                throw new ArgumentNullException(nameof(newPayout), "The payout cannot be null.");
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
            string requestUrl = "https://saacapps.com/payout/payout.php";  // Adjust this URL according to your actual endpoint

            // Convert the newContact object to JSON
            string jsonContent = JsonConvert.SerializeObject(newPayout);

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
                var result = JsonConvert.DeserializeObject<PayoutDto>(await response.Content.ReadAsStringAsync());
                return result;
            }
            else
            {
                // Capture the error content to diagnose the problem
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating the contact: {response.StatusCode} - {response.ReasonPhrase}. Details: {errorContent}");
            }
        }
    }
}