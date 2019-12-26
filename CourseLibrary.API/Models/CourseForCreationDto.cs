using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{

    public class CourseForCreationDto : CourseManipulationDto //: IValidatableObject 
    {

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("El Campo descripcion debe ser Diferente al Campo Title", new[] { "CourseForCreationDtl" });

        //    }
        //}
    
    }
}
