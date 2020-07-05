using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Facebook.Models
{
    public class Address
    {

        public string City { get; set; }
        [Required]
        public Country Country { get; set; }

        public int Zipcode { get; set; }

        public override string ToString()
        {
            return $"{City}, {Country}";
        }

    }
}