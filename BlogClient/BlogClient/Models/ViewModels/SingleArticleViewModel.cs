using BlogClient.DTO;

namespace BlogClient.Models.ViewModels
{
    public class SingleArticleViewModel
    {
        public ArticleDetailsDto articleDetails { get; set; }
        public List<CommentWithUserDto> commentWithUsers { get; set; }
        public ArticleDto articleDto { get; set; }
    }
}
