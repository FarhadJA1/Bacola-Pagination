using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bacola_MVC_Updated_.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Offer { get; set; }
        [Required]
        public string UpTitle { get; set; }
        [Required]
        public string DownTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
