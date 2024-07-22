using System.Net.Http.Json;
using System.Text.Json;

using OAuthPrimer;

using static OAuthPrimer.Helper;

const int port = 3000;

// 1. First step is to register your application:
//    1. https://learn.microsoft.com/en-us/graph/auth/auth-concepts#register-the-application
//    2. Give it a nice name and select the supported account types as
//       Personal Microsoft accounts. Mine is called oauth-primer.
//    3. Set the redirect URI type "public client" (this program!) and URI
//       as http://localhost:{port}/oauth-callback.
//    4. Take note of the Application (client) ID

var authenticationCodeParameters = new Dictionary<string, string>
{
	//
	// ⚒️ T O D O: Fill with the query parameters of a OAuth authentication code flow request
	//             (see https://learn.microsoft.com/en-us/graph/auth-v2-user?tabs=http#authorization-request)
	//
	TODO
};

// Let's direct the user to authenticate:
var builder = new UriBuilder($"https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize");

foreach (var kvp in authenticationCodeParameters)
{
	builder.AddQueryParameter(kvp.Key, kvp.Value);
}

// Handle the authentication callback
var codeTask = CaptureAuthorizationCallback(request =>
{
	// Handle authentication errors
	if (request.QueryString["error"] != null)
	{
		throw new Exception($"{request.QueryString["error"]}: {request.QueryString["error_description"]}");
	}

	//
	// ⚒️ T O D O: Write here code to handle the http request and extract the token from the query parameters
	//
	var param = TODO;

	return param ?? throw new Exception("No such param");
}, port);

// Open the browser to authenticate and capture the credentials
OpenBrowser(builder.Uri);

// IMPORTANT - await **after** opening the browser to avoid racing conditions.
var code = await codeTask;

// Now you need to trade the authentication code for an access token
// For this, you need to make another request to authentication server.
using var httpClient = new HttpClient();

var response = await httpClient.PostAsync(
	$"https://login.microsoftonline.com/consumers/oauth2/v2.0/token",
	new FormUrlEncodedContent(new Dictionary<string, string>
	{
		//
		// ⚒️ T O D O : Fill in the details of the request
		//
		TODO
	}));

response.EnsureSuccessStatusCode();

var responseData = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>()
	?? throw new Exception("Error desserializing access token response - maybe you forgot something??");

// 
// ⚒️ T O D O: Extract the access token from the response
//
var accessToken = TODO; 


// Now we can query the API with the access token
var meRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");

//
// ⚒️ T O D O: Adjust the request to avoid the authentication error
//
TODO;

var meResponse = await httpClient.SendAsync(meRequest);
meResponse.EnsureSuccessStatusCode();

// Parse the response and display it to the user
var me = await meResponse.Content.ReadFromJsonAsync<Profile>()
	?? throw new Exception("Error reading gists");

Console.WriteLine("-------------- PROFILE -------------------------");
Console.WriteLine(me);
Console.WriteLine("");

var mailRequest = new HttpRequestMessage(
	HttpMethod.Get, 
	"https://graph.microsoft.com/v1.0/me/messages?$select=subject,receivedDateTime");
//
// ⚒️ T O D O: Adjust the request to avoid the authentication error, **same as above**
//
TODO;

var mailResponse = await httpClient.SendAsync(mailRequest);
mailResponse.EnsureSuccessStatusCode();

// Parse the response and display it to the user
var mailODataContent = await mailResponse.Content.ReadFromJsonAsync<ODataResponse<Mail>>(JsonOptions())
	?? throw new Exception("Error reading gists");

const int columnSize = 50;

Console.WriteLine("-------------- EMAIL -------------------------");
Console.WriteLine($"| {"Subject",-columnSize} | {"Date",-columnSize} |");
Console.WriteLine($"| {new string('-', columnSize),-columnSize} | {new string('-', columnSize),-columnSize} |");

foreach (var mail in mailODataContent.Value)
{
	Console.WriteLine($"| {mail.Subject.TrimTo(columnSize),-columnSize} | {mail.ReceivedDateTime,-columnSize} |");
}

static JsonSerializerOptions JsonOptions() => new()
{
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};