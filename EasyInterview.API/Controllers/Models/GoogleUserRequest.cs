using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasyInterview.API.Controllers.Models
{
    public class GoogleUserRequest
    {
        public const string PROVIDER = "google";

        [JsonProperty("idToken")]
        [Required]
        public string IdToken { get; set; }
    }
}
