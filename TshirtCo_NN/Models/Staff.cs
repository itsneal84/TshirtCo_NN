using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Staff : ApplicationUser
    {
        
        public Guid StaffId { get; set; }
    }
}
