using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    /// <summary>
    /// extension for stripe to retrieve charge id from transaction
    /// </summary>
    public class ChargeViewModel
    {
        public string ChargeId { get; set; }
    }
}
