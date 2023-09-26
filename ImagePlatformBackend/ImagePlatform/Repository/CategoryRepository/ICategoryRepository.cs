using ImagePlatform.Models;

namespace ImagePlatform.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category GetCategory(Guid id);
        Category AddCategory(Category category);
        void DeleteCategory(Category category);
        Category UpdateCategory(Category category);

    }
}
