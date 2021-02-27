using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.DataAccess.Entities
{
    [Table("users")]
    public class OidcUserEntity : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public List<InterviewEntity> Interviews { get; set; }
    }
}
