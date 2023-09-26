using System.ComponentModel.DataAnnotations;

namespace ImagePlatform.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}
