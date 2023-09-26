using ImagePlatform.Models;
using ImagePlatform.Models.Request;

namespace ImagePlatform.Services.PostService
{
    public interface IPostService
    {
        Post GetPost(Guid postId);
        Post AddPost(PostRequest request);
        List<Post> GetPosts(string? search, Guid? categoryId);
    }
}
