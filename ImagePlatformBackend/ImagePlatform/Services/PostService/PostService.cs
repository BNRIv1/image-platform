using ImagePlatform.Models;
using ImagePlatform.Models.Request;
using ImagePlatform.Repository.CategoryRepository;
using ImagePlatform.Repository.Interfaces;
using ImagePlatform.Repository.UserRepository;

namespace ImagePlatform.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository categoryRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            this.postRepository = postRepository;
            _userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        public Post AddPost(PostRequest request)
        {
            var user = _userRepository.GetUser(request.AuthorId);
            var category = categoryRepository.GetCategory(request.CategoryId);

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Downloads = 0,
                Views = 0,
                DatePublished = DateTime.Now,
                Description = request.Description,
                Author = user,
                AuthorId = user.Id,
                IsPremium = request.IsPremium,
                Category = category,
                CategoryId = category.Id,
            };
            postRepository.AddPost(post);
            return post;
        }

        public Post GetPost(Guid postId)
        {
            var post = postRepository.GetPostById(postId);
            post.Views++;
            postRepository.UpdatePost(post);
            return post;
        }

        public List<Post> GetPosts(string? search, Guid? categoryId)
        {
            if(search != null && categoryId != null)
            {
                return postRepository.GetPostsByCategoryIdAndDescription((Guid)categoryId, search).ToList();
            }else if (categoryId != null)
            {
                return postRepository.GetPostsByCategoryId((Guid)categoryId).ToList();
            }else if (search != null )
            {
                return postRepository.GetPostsByDescription(search).ToList();
            }
            else
            {
                return postRepository.GetPosts().ToList();
            }
        }
    }
}
