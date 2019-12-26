using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    public class AuthorForCreationDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string MainCategory { get; set; }

        public ICollection<CourseForCreationDto> courses { get; set; }
        = new List<CourseForCreationDto>();

    }
}
