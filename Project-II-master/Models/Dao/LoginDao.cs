using Newtonsoft.Json;
using Project_II.Models.Dto;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project_II.Models.Dao
{

        public class LoginDao
        {
            // URL of the authentication API
            private readonly string apiBaseUrl = "https://saacapps.com/payout/auth.php";

            // Static property to store the token globally
            public static string Token { get; private set; }

            // Method to authenticate the user
            public async Task<AuthDto> AuthenticateUserAsync(LoginDto model)
            {
                var client = new HttpClient();

                // Concatenate the username and password with ":" and then encode in Base64
                var credentials = $"{model.Username}:{model.Password}";
                var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

                // Create the HTTP request
                var request = new HttpRequestMessage(HttpMethod.Post, apiBaseUrl);

                // Add the Authorization header with the Base64 encoded value
                request.Headers.Add("Authorization", "Basic " + encodedCredentials);

                // Perform the POST request to the API
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the API response (which contains the token)
                    var result = JsonConvert.DeserializeObject<AuthDto>(await response.Content.ReadAsStringAsync());

                    // Store the token in the static property
                    Token = result?.Token;

                    return result;
                }

                // If the response is not successful, return null
                return null;
            }

            // Method to validate the session (if necessary)
            public async Task<bool> IsTokenValidAsync(string token = null)
            {
                var client = new HttpClient();

                // If no token is passed as a parameter, use the global token
                string tokenToValidate = token ?? Token;

                if (string.IsNullOrEmpty(tokenToValidate))
                {
                    // If there is no token, the session cannot be validated
                    return false;
                }

                // Add the Authorization header with the token for validation
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenToValidate);

                var validateSessionUrl = "https://saacapps.com/payout/validate-session.php"; // URL for session validation
                var response = await client.GetAsync(validateSessionUrl);

                return response.IsSuccessStatusCode; // If the status code is 200, the token is valid
            }
        }
    }
