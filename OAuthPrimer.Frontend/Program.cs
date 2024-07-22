using System.Net.Http.Json;

// 1. First step is to register your application:
//    1. https://learn.microsoft.com/en-us/graph/auth/auth-concepts#register-the-application
//    2. Give it a nice name and select the supported account types as
//       Personal Microsoft accounts. Mine is called oauth-primer.
//    3. Set the redirect URI type "public client" (this program!) and URI
//       as http://localhost:{port}/oauth-callback.
//    4. Take note of the Application (client) ID and the tenant ID
const string tenantId = TODO;
const int backendPort = 5019;

// Let's authenticate to the authentication server
var authenticationClient = new HttpClient();
var builder = new UriBuilder($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token");

var authenticationResponse = await authenticationClient.PostAsync(builder.Uri, new FormUrlEncodedContent(new Dictionary<string, string>
{
    //
    // ⚒️ T O D O: Fill with the query parameters of a OAuth client credentials request
    //             Don't forget the scope.
    //
}));

authenticationResponse.EnsureSuccessStatusCode();

var authContent = await authenticationResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();

//
// ⚒️ T O D O: Retrieve the token from the response
// 
var accessToken = TODO;

// Once you have the token, all you need to do is call the backend
var backendClient = new HttpClient();

var weatherRequest = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:{backendPort}/WeatherForecast");

//
// ⚒️ T O D O: Adjust the request to avoid the authentication error
//

var weatherResponse = await backendClient.SendAsync(weatherRequest);

weatherResponse.EnsureSuccessStatusCode();

// Parse the response and display it to the user
var weather = await weatherResponse.Content.ReadAsStringAsync()
    ?? throw new Exception("Error reading response");

Console.WriteLine("-------------- WEATHER -------------------------");
Console.WriteLine(weather);
