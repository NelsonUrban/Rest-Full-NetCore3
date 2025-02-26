﻿using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    
    public class CourseForUpdateDto : CourseManipulationDto
    {
        [Required(ErrorMessage ="El campo descripcion es Obligatorio")]
        public override string Description {

                    get => base.Description; 
                    set => base.Description = value;
        }
    }
}
