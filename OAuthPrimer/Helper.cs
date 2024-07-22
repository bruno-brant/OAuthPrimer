using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OAuthPrimer;

/// <summary>
/// Helper methods to simplify the exercise.
/// </summary>
public static class Helper
{
	/// <summary>
	///		Starts a new <see cref="HttpListener"/> and waits 
	///		for a request to be made to the specified port and path.
	/// </summary>
	/// <param name="handler">
	///		Method called when the request is received to process it
	///		and return the desired data.
	///	</param>
	/// <param name="port">
	///		The port where the listener should run.
	/// </param>
	/// <returns>
	///		The result of the handler method.
	/// </returns>
	public static Task<T> CaptureAuthorizationCallback<T>(Func<HttpListenerRequest, T> handler, int port = 3000, string path = "", string title = "OAuthPrimer")
	{
		var httpListener = new HttpListener();
		httpListener.Prefixes.Add($"http://localhost:{port}/{path}");

		return Task.Run(async () =>
		{
			httpListener.Start();

			var context = await httpListener.GetContextAsync();
			var request = context.Request;
			var response = context.Response;

			try
			{
				var token = handler(request);

				response.StatusCode = 200;
				response.WriteHtmlOutput(
					$"<title>{title}</title>",
					"<div><h1>Success!</h1><p>You can close this browser tab now.</p></div>");

				return token;
			}
			catch (Exception ex)
			{
				response.StatusCode = 500;
				response.WriteHtmlOutput(
					$"<title>{title}</title>",
					$"<div><h1>Authorization error!</h1><p style={{foreground: 'red', font-style:'bold'}}>{ex.Message}</p></div>");

				throw;
			}
		});
	}

	/// <summary>
	/// Opens a browser at the specified URL.
	/// </summary>
	/// <param name="url">The URL to be opened at.</param>
	/// <returns>True if the browser opened successfully, false otherwise.</returns>
	/// <remarks>
	/// Writes to the console if the browser could not be opened.
	/// </remarks>
	public static bool OpenBrowser(Uri url)
	{
		try
		{
			var _ = Process.Start(new ProcessStartInfo
			{
				FileName = url.ToString(),
				UseShellExecute = true
			}) ?? throw new Exception("Failed to open browser");

			return true;
		}
		catch
		{
			Console.WriteLine("Failed to open the browser automatically.");
			Console.WriteLine($"Please open the following url in your browser: {url}");

			return false;
		}
	}

	/// <summary>
	/// Default <see cref="JsonSerializerOptions"/> to use in the application.
	/// </summary>
	public static readonly JsonSerializerOptions SerializerOptions = new()
	{
		AllowTrailingCommas = true,
		Converters = { new JsonStringEnumConverter() },
		PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
	};
}
