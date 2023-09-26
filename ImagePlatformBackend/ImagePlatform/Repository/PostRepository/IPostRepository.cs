using ImagePlatform.Models;

namespace ImagePlatform.Repository.Interfaces
{
    public interface IPostRepository
    {
        ICollection<Post> GetPosts();
        Post GetPostById(Guid id);
        ICollection<Post> GetPostsByDescription(string description);
        bool PostExists(Guid id);
        ICollection<Post> GetPostsByAuthorId(Guid authorId);
        ICollection<Post> GetPremiumPosts();
        Post AddPost(Post post);
        void DeletePostsByCategoryId(Guid categoryId);
        Post UpdatePost(Post post);
        List<Post> GetPostsByCategoryId(Guid id);
        List<Post> GetPostsByCategoryIdAndDescription(Guid id, string description);
    }
}
