using ImagePlatform.Models;
using ImagePlatform.Models.Request;
using ImagePlatform.Repository.CategoryRepository;
using ImagePlatform.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImagePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            this.categoryRepository = categoryRepository;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = categoryRepository.GetCategories();
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpPost("add")]
        public IActionResult AddCategory(CategoryRequest request)
        {
            var category = categoryService.AddCategory(request);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpPost("update")]
        public IActionResult UpdateCategory(Category category)
        {
            categoryRepository.UpdateCategory(category);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCategory(Guid id) {
            var category = categoryRepository.GetCategory(id);
            categoryRepository.DeleteCategory(category);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
