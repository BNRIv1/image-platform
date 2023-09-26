using ImagePlatform.Models;
using ImagePlatform.Models.Request;

namespace ImagePlatform.Services.CategoryService
{
    public interface ICategoryService
    {
        Category AddCategory(CategoryRequest request);
    }
}
