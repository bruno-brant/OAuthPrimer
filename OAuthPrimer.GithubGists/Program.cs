using System.Net.Http.Json;

using OAuthPrimer;

using static OAuthPrimer.Helper;

const int port = 3001;

var authenticationCodeParameters = new Dictionary<string, string>
{
	//
	// ⚒️ TODO: Fill with the query parameters of a OAuth authentication code flow request
	//          (see https://docs.github.com/en/developers/apps/building-oauth-apps/authorizing-oauth-apps)
	//
	TODO,
};

// Let's direct the user to authenticate:
var builder = new UriBuilder("https://github.com/login/oauth/authorize");

foreach (var kvp in authenticationCodeParameters)
{
	builder.AddQueryParameter(kvp.Key, kvp.Value);
}

var codeTask = CaptureAuthorizationCallback(request =>
{
	// !! TODO: Write here code to handle the http request and extract the token from the query parameters
	var param = request.QueryString["code"];

	return param ?? throw new Exception("No such param");
}, port);

OpenBrowser(builder.Uri);

// IMPORTANT - await _after_ opening the browser to avoid racing conditions.
var code = await codeTask;

// Now you need to trade the authentication code for an access token
// For this, you need to make another request to the Github API. 

using var httpClient = new HttpClient();

//
// T O D O : Fill in the details of the request
//
var response = await httpClient.PostAsync(
	"https://github.com/login/oauth/access_token",
	new FormUrlEncodedContent(new Dictionary<string, string>
	{
		TODO,
	}));

response.EnsureSuccessStatusCode();

var responseData = await response.Content.ReadFromFormUrlEncoded()
	?? throw new Exception("Error desserializing access token response - maybe you forgot something?");

// 
// ⚒️ T O D O: Extract the access token from the response
//
var accessToken = TODO; 

// Now we can query the API with the access token

var gistRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/gists");
gistRequest.Headers.UserAgent.Add(new("Wondrous-CLI-Gist-Application", null));
gistRequest.Headers.Accept.Add(new("application/vnd.github+json"));

//
// ⚒️ T O D O: Adjust the request to avoid the authentication error
//
TODO;

var gistsResponse = await httpClient.SendAsync(gistRequest);

gistsResponse.EnsureSuccessStatusCode();

// Getting the response content
var gists = await gistsResponse.Content.ReadFromJsonAsync<List<Gist>>(SerializerOptions)
	?? throw new Exception("Error reading gists");

foreach (var gist in gists) Console.WriteLine(gist);
