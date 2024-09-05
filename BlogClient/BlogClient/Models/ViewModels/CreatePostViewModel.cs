using BlogClient.DTO;

namespace BlogClient.Models.ViewModels
{
    public class CreatePostViewModel
    {
        public List<CategoryDTO> Categories { get; set; }
        public List<TagDTO> Tags { get; set; }
        public ArticlePostDto articleDTO { get; set; }
    }
}
