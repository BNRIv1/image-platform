using System.ComponentModel.DataAnnotations;

namespace ImagePlatform.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string? ImageUrl{ get; set; }
        public DateTime DatePublished{ get; set; }
        public string Description { get; set; }
        public long Views { get; set; }
        public long Downloads { get; set; }
        public bool IsPremium { get; set; }
        public Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}
