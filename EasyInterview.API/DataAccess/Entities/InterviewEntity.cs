using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EasyInterview.API.Controllers.Models;

namespace EasyInterview.API.DataAccess.Entities
{
    [Table("interviews")]
    public class InterviewEntity : IEntity
    {
        public int Id { get; set; }
        public AppUser Owner { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime BookedDate { get; set; }
    }
}
