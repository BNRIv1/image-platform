using ImagePlatform.Models;
using ImagePlatform.Models.Request;
using ImagePlatform.Repository.CategoryRepository;

namespace ImagePlatform.Services.CategoryService
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public Category AddCategory(CategoryRequest request)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Title = request.Title
            };

            categoryRepository.AddCategory(category);
            return category;
        }
    }
}
