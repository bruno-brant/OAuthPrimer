using System.Net;
using System.Web;

namespace OAuthPrimer;

/// <summary>
/// Multiple extension methods to simplify the exercise.
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Adds a query parameter to the <see cref="UriBuilder"/>.
	/// </summary>
	/// <param name="builder">The builder where the parameters will be added to.</param>
	/// <param name="name">The name of the parameter.</param>
	/// <param name="value">The value of the parameter.</param>
	/// <returns>The <see cref="UriBuilder"/> instance to allow for method chaining.</returns>
	public static UriBuilder AddQueryParameter(this UriBuilder builder, string name, string value)
	{
		builder.Query = string.IsNullOrEmpty(builder.Query)
			? $"{name}={value}"
			: $"{builder.Query}&{name}={HttpUtility.UrlEncode(value)}";

		return builder;
	}

	/// <summary>
	/// Writes an html output to the <see cref="HttpListenerResponse"/>.
	/// </summary>
	/// <param name="response">The response that the html will be written to.</param>
	/// <param name="head">The contents of the head tag.</param>
	/// <param name="body">The contents of the body tag.</param>
	public static void WriteHtmlOutput(this HttpListenerResponse response, string head = "", string body = "")
	{
		response.ContentType = "text/html";

		var html = $"<html><head>{head}</head><body>{body}</body></html>";

		response.OutputStream.Write(System.Text.Encoding.UTF8.GetBytes(html));
	}

	/// <summary>
	/// Reads form url encoded content and returns a dictionary with the values.
	/// </summary>
	/// <param name="content">The content to be read from.</param>
	/// <returns>A dictionary containing the decoded values of the content.</returns>
	public static async Task<Dictionary<string, string>> ReadFromFormUrlEncoded(this HttpContent content)
	{
		var body = await content.ReadAsStringAsync();

		return body.Split("&")
			.Select(nameValue => nameValue.Split("="))
			.ToDictionary(x => x[0], x => HttpUtility.UrlDecode(x[1]));
	}

	/// <summary>
	/// Trims a string to a maximum length and adds an ellipsis if it exceeds it.
	/// </summary>
	/// <param name="str"></param>
	/// <param name="length"></param>
	/// <returns></returns>
	public static string TrimTo(this string str, int length = 50) =>
		str.Length > length ? str.Substring(0, length - 3) + "..." : str;
}