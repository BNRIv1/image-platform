using ImagePlatform.Models;
using ImagePlatform.Models.Request;
using ImagePlatform.Repository.Interfaces;
using ImagePlatform.Repository.UserRepository;
using ImagePlatform.Services.PostService;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net.Http.Headers;

namespace ImagePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController: Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostService _postService;
        private readonly string uploadPath = "wwwroot/uploads";

        public PostController(IPostRepository postRepository, IUserRepository userRepository,
            IPostService postService)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _postService = postService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPosts(string? search, Guid? category)
        {
            var posts = _postService.GetPosts(search, category);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Post))]
        [ProducesResponseType(400)]
        public IActionResult GetPost(Guid id)
        {
            if (!_postRepository.PostExists(id))
            {
                return NotFound();
            }
            
            var post = _postService.GetPost(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(post);
        }

        [HttpGet("UserPosts/{authorId}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        public IActionResult GetPostsByAuthorId(Guid authorId)
        {
            var posts = _postRepository.GetPostsByAuthorId(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(posts);
        }

        [HttpGet("premium")]
        [ProducesResponseType(200, Type = typeof(Post))]
        public IActionResult GetPostsWherePremiumTrue()
        {
            var posts = _postRepository.GetPremiumPosts();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(posts);
        }

        [HttpPost("add")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult AddPost([FromForm]PostRequest request)
        {

            if (request.Image == null || request.Image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var post = _postService.AddPost(request);

            var fileName = post.Id.ToString() + Path.GetExtension(request.Image.FileName);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                request.Image.CopyToAsync(stream);
            }


            post.ImageUrl = filePath.Substring(8).Replace(@"\", "/");
            _postRepository.UpdatePost(post);

            return Ok("Data uploaded successfully.");



        }
    }
}
