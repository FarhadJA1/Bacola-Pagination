using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bacola_MVC_Updated_.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public int DIscount { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
