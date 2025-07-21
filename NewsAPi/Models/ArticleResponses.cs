namespace NewsAPi.Models;

public class ArticleResponses
{
    public string Status { get; set; }
    public int TotalResults { get; set; }
    public List<Article> Articles { get; set; }
}