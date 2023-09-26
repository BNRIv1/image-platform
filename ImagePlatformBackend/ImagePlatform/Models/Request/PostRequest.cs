namespace ImagePlatform.Models.Request
{
    public class PostRequest
    {
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
