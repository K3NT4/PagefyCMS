namespace PagefyCMS.Models
{
    public class ArticlePage
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
