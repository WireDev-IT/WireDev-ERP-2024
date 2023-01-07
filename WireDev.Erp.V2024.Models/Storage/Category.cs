using System;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
	public class Category
	{
        public Category()
        {
            Uuid = Guid.NewGuid();
        }

        [Key]
        public Guid Uuid { get; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}