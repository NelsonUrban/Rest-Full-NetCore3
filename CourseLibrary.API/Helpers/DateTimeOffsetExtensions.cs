using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currenDate = DateTime.UtcNow;

            int age = currenDate.Year - dateTimeOffset.Year;

            if (currenDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        
        }

    }
}
