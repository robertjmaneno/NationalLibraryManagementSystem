using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib.CoreService.Lib.Common;
using NaLib.CoreService.Lib.Data;

namespace NaLib.CoreService.API.Controllers
{
    [Route(ApiUrls.StaffDetails)]
    [ApiController]
    public class StaffDetailsController : ControllerBase
    {

        private readonly NaLibCoreServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public StaffDetailsController(NaLibCoreServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new skill.
        /// </summary>
        /// <param name="request">The request data containing the skill details.</param>
        /// <returns>A response indicating whether the skill was created successfully.</returns>
        /// <response code="201">The skill was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateSkills)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var skill = new Skill
            {
                SkillName = request.SkillName,
                Description = request.Description,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return this.SendApiResponse("Skill created successfully.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Creates a new grade.
        /// </summary>
        /// <param name="request">The request data containing the grade details.</param>
        /// <returns>A response indicating whether the grade was created successfully.</returns>
        /// <response code="201">The grade was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateGrades)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]

        [Authorize]
        public async Task<IActionResult> CreateGrade([FromBody] CreateGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var grade = new Grade
            {
                GradeName = request.GradeName,
                Description = request.Description,
                UserId = request.UserId,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return this.SendApiResponse("Grade created successfully.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a new qualification.
        /// </summary>
        /// <param name="request">The request data containing the qualification details.</param>
        /// <returns>A response indicating whether the qualification was created successfully.</returns>
        /// <response code="201">The qualification was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateQualifications)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQualification([FromBody] CreateQualificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var qualification = new Qualification
            {
                QualificationName = request.QualificationName,
                Description = request.Description,
                UserId = request.UserId,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Qualifications.Add(qualification);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return this.SendApiResponse("Qualification created successfully.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Creates a new experience.
        /// </summary>
        /// <param name="request">The request data containing the experience details.</param>
        /// <returns>A response indicating whether the experience was created successfully.</returns>
        /// <response code="201">The experience was created successfully.</response>
        /// <response code="400">Validation error in the request data.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.CreateExperiences)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var experience = new Experience
            {
                YearsOfExperience = request.YearsOfExperience,
                UserId = request.UserId,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Experiences.Add(experience);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return this.SendApiResponse("Experience created successfully.", null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError<object>(
                    null,
                    "DatabaseError",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Assigns a skill to a user using the user's ID and skill name.
        /// </summary>
        /// <param name="request">The request data containing the user ID and skill name.</param>
        /// <returns>A response indicating whether the skill was assigned successfully.</returns>
        /// <response code="200">The skill was assigned successfully.</response>
        /// <response code="400">Validation error or skill not found.</response>
        /// <response code="500">Internal server error during database operation.</response>
        [HttpPost(ApiUrls.AssignSkillToUser)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignSkillToUser([FromBody] AssignSkillToUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            try
            {

                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.SkillName == request.SkillName);

                if (skill == null)
                {
                    return this.SendApiError<object>(
                                                                                                                      null,
     "UnhandledError",
     "An unexpected error occurred.",
     StatusCodes.Status500InternalServerError
 );

                }


                var userSkill = new UserSkill
                {
                    UserId = request.UserId,
                    SkillId = skill.Id,
                 
                };

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.UserSkills.Add(userSkill);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return this.SendApiResponse("Skill assigned to user successfully.", null);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return this.SendApiError<object>(
                        null,
                        "DatabaseError",
                        "An error occurred while assigning the skill.",
                        StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return this.SendApiError<object>(
                    null,
                    "UnhandledError",
                    "An unexpected error occurred.",
                    StatusCodes.Status500InternalServerError);
            }
        }

    }
}
