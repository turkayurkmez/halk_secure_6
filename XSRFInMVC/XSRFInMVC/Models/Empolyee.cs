using System.ComponentModel.DataAnnotations;

namespace XSRFInMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "İsim boş olamaz")]
        [MinLength(3, ErrorMessage = "En az üç harfli olmalı")]
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
