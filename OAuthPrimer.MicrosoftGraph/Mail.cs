using System.Text.Json.Serialization;

public record ODataResponse<T>(
	[property: JsonPropertyName("@odata.context")] string ODataContext, 
	T[] Value, 
	[property: JsonPropertyName("@odata.nextLink")]string ODataNextLink);

public record Mail(
	string Id,
	string ReceivedDateTime,
	string Subject);