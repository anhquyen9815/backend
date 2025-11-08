using System;
using System.ComponentModel.DataAnnotations;

namespace DienMayLongQuyen.Api.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Image { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } 
    }
}
