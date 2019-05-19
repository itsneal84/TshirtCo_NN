using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TshirtCo_NN.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLn1 { get; set; }
        public string AddressLn2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}
