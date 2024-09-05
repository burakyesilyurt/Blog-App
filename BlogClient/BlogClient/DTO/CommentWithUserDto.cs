namespace BlogClient.DTO
{
    public class CommentWithUserDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
