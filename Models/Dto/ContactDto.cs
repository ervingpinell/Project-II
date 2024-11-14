using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_II.Models.Dto
{
    public class ContactDto
    {
        public int Id {  get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Client_id { get; set; }

    }
}