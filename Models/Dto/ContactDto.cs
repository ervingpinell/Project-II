using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_II.Models.Dto
{
    public class ContactDto
    {
        public int id {  get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone{ get; set; }
        public string email { get; set; }
        public int client_id { get; set; }

    }
}