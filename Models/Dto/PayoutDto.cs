using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_II.Models.Dto
{
    public class PayoutDto
    {
        public int id { get; set; }
        public int contact_id { get; set; }
        public int amount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The amount must be a positive number.")]
        [DataType(DataType.Currency)]
        public string status { get; set; }
        public DateTime create_date { get; set; }

    }
}