using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TshirtCo_NN.Models
{
    /// <summary>
    /// application user inherits from the identity user class
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// extending identityUser's properties
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLn1 { get; set; }
        public string AddressLn2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

    }
}
