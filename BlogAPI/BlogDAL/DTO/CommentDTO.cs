namespace BlogDAL.DTO
{
    public class CommentDTO
    {
        public int? CommentId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }

    }
}
