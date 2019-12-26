using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute(
        ErrorMessage = "El campo titulo no puede ser igual al campo descripcion")]
    public abstract class CourseManipulationDto
    {

        [Required(ErrorMessage = "Debe Ingresar un Titulo este campo es Requerido")]
        [MaxLength(100, ErrorMessage = "El titulo solo Admite 100 Caracteres")]
        public  string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "La Descripcion solo Admite 100 Caracteres")]
        public virtual string Description { get; set; }

    }
}
