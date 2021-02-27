using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Controllers.Models
{
    public class CreateOidcUserModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
