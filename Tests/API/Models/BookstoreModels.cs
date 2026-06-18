using Newtonsoft.Json;

namespace Tests.API.Models;

public class Book
{
    [JsonProperty("isbn")]
    public string Isbn { get; set; } = string.Empty;

    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonProperty("subTitle")]
    public string SubTitle { get; set; } = string.Empty;

    [JsonProperty("author")]
    public string Author { get; set; } = string.Empty;

    [JsonProperty("publish_date")]
    public DateTime PublishDate { get; set; }

    [JsonProperty("publisher")]
    public string Publisher { get; set; } = string.Empty;

    [JsonProperty("pages")]
    public int Pages { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("website")]
    public string Website { get; set; } = string.Empty;
}

public class BooksResponse
{
    [JsonProperty("books")]
    public List<Book> Books { get; set; } = new();
}
