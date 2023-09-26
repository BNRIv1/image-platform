using ImagePlatform.Data;
using ImagePlatform.Models;
using ImagePlatform.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImagePlatform.Repository.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private DbSet<Post> _posts;

        public PostRepository(DataContext context)
        {
            _context = context;
            _posts = context.Set<Post>();
        }

        public Post AddPost(Post post)
        {
            _posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        public List<Post> GetPostsByCategoryId(Guid id)
        {
            return _posts.Where(p => p.CategoryId == id).ToList();
        }

        public List<Post> GetPostsByCategoryIdAndDescription(Guid id, string description)
        {
            return _posts.Where(p => p.CategoryId == id && p.Description == description).ToList();
        }

        public Post GetPostById(Guid id)
        {
            return _posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Post> GetPosts()
        {
            return _posts.ToList();
        }

        public ICollection<Post> GetPostsByAuthorId(Guid authorId)
        {
            return _posts.Where(p => p.AuthorId == authorId).ToList();
        }

        public ICollection<Post> GetPostsByDescription(string description)
        {
            return _context.Posts
                .Where(p => p.Description.ToLower().Contains(description.ToLower()))
                .ToList();
        }

        public ICollection<Post> GetPremiumPosts()
        {
            return _context.Posts.Where(p => p.IsPremium == true).ToList();
        }

        public bool PostExists(Guid id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public Post UpdatePost(Post post)
        {
            _posts.Update(post);
            _context.SaveChanges();
            return post;
        }

        public void DeletePostsByCategoryId(Guid categoryId)
        {
            _posts.RemoveRange(_posts.Where(p => p.CategoryId == categoryId));
        }
    }
}
