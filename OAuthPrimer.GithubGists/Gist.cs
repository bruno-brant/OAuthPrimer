/// <summary>
/// A Github gist in the API.
/// </summary>
/// <param name="Url">The URL of the gist.</param>
/// <param name="ForksUrl">The URL to fetch the forks of the gist.</param>
/// <param name="CommitsUrl">The URL to fetch the commits of the gist.</param>
/// <param name="Id">The ID of the gist.</param>
/// <param name="NodeId">The node ID of the gist.</param>
/// <param name="HtmlUrl">The HTML URL of the gist.</param>
/// <param name="Files">The dictionary of files within the gist.</param>
/// <param name="Public">A flag indicating if the gist is public or not.</param>
/// <param name="Description">The description of the gist.</param>
/// <param name="Comments">The number of comments on the gist.</param>
/// <param name="CommentsUri">The URI to fetch the comments of the gist.</param>
internal record Gist(
	Uri Url,
	Uri ForksUrl,
	Uri CommitsUrl,
	string Id,
	string NodeId,
	Uri HtmlUrl,
	Dictionary<string, GistFile> Files,
	bool Public,
	string Description,
	int Comments,
	Uri CommentsUri)
{
	public override string ToString()
	=> $"""
		{Description}
		{new string('-', Description.Length)}
			Url: {Url}
			This is a {(Public ? "public" : "private")} gist.
			Files({Files.Count}): 
			 - {string.Join("\n\t - ", Files.Select(kvp => $"{kvp.Key}: {kvp.Value.RawUrl}"))}

		""";
}

/// <summary>
/// Represents a file within a Github gist.
/// </summary>
/// <param name="Filename">The name of the file.</param>
/// <param name="Type">The type of the file.</param>
/// <param name="Language">The programming language of the file.</param>
/// <param name="RawUrl">The raw URL of the file.</param>
/// <param name="Size">The size of the file in bytes.</param>
internal record GistFile(
	string Filename,
	string Type,
	string Language,
	Uri RawUrl,
	int Size);
