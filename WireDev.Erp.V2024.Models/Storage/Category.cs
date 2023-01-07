using System;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
	public class Category
	{
        public Category(Guid id, string name, string? description, string? color)
        {
            Id = id;
            Name = name;
            Description = description;
            Color = color;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}