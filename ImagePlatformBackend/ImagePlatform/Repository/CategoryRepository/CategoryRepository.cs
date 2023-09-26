using ImagePlatform.Data;
using ImagePlatform.Models;
using ImagePlatform.Repository.CategoryRepository;
using ImagePlatform.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImagePlatform.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IPostRepository postRepository;
        private DbSet<Category> _categories;

        public CategoryRepository(DataContext dataContext, IPostRepository postRepository)
        {
            _context = dataContext;
            this.postRepository = postRepository;
            _categories = _context.Set<Category>();
        }
        public Category AddCategory(Category category)
        {
            _categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public void DeleteCategory(Category category)
        {
            postRepository.DeletePostsByCategoryId(category.Id);
            _categories.Remove(category);
            _context.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return _categories.ToList();    
        }

        public Category GetCategory(Guid id)
        {
            return _categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category UpdateCategory(Category category)
        {
            _categories.Update(category);
            _context.SaveChanges();
            return category;
        }
    }
}
