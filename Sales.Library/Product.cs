﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library
{
    public class Product
    {
        [Key] public int Id { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? Description { get; set; }
        [Required] public double Price { get; set; }
        [Required] public DateTime CreatedDate { get; set; }
        [Required] public string? Image { get; set; }
        [Required] public int SubCategoryId { get; set; }
        [ForeignKey(nameof(SubCategoryId))] public SubCategory? SubCategory { get; set; }
    }
}