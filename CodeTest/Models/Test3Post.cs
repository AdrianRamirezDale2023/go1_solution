using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruebaIngreso.Models
{
    public class Test3Post
    {
        [Required(ErrorMessage = "El campo código es requerido")]
        [Display(Name = "Código")]
        public string Code { get; set; }
    }
}