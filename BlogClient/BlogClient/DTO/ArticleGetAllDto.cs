namespace BlogClient.DTO
{
    public class ArticleGetAllDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public string FullName { get; set; }
        public int Comment { get; set; }
        public int ArticleLikes { get; set; }

    }
}
