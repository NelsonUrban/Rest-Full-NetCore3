using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
             throw new ArgumentNullException(nameof(courseLibraryRepository));

        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCourseForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourses(authorId);
             return Ok(_mapper.Map<IEnumerable<CourseDto>>(courseForAuthorFromRepo));
        }
        [HttpGet("{courseId}",Name ="GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor( Guid authorId , Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseDto>(courseForAuthorFromRepo));

        }
        [HttpPost]
        public ActionResult<CourseDto> CreateCourse(Guid authorId,CourseForCreationDto course)
        {
            if (! _courseLibraryRepository.AuthorExists(authorId)) return NotFound();

            var courseEntity = _mapper.Map<Entities.Course>(course);
            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            _courseLibraryRepository.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);         
            return CreatedAtRoute("GetCourseForAuthor",
                new { authorId = authorId, courseId = courseToReturn.Id },
                  courseToReturn);
        }
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId,
            Guid courseId,
            CourseForUpdateDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();

            var courseForAurthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAurthorFromRepo == null)
            {
                var courseAdd = _mapper.Map<Entities.Course>(course);
                courseAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseAdd);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId = authorId, courseId = courseToReturn.Id },
                      courseToReturn);
            }
            _mapper.Map(course, courseForAurthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAurthorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();

        }
        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId,
            Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null) 
            {
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto,ModelState);

                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }
                var courseAdd = _mapper.Map<Entities.Course>(courseDto);
                courseAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseAdd);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                  new { authorId = authorId, courseId = courseToReturn.Id },
                    courseToReturn);

            }

            var coursePatch = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);
            // add validation
            patchDocument.ApplyTo(coursePatch,ModelState);

            if (!TryValidateModel(coursePatch)) // validando modelo JSonPatchDocument
            {
                return ValidationProblem(ModelState);
            } 

            _mapper.Map(coursePatch, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();

        }
        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();

            }
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null) return NotFound();

            _courseLibraryRepository.DeleteCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        
        }
        public override ActionResult ValidationProblem( [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }


    }
}
