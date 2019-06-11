using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    /// <summary>
    /// contact form for users to send a message to the admin
    /// </summary>
    public class ContactForm
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
