using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Reviews;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MovieReviewsServices;
using CinemaSystem.WebApi.CustomFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/movies/{movieId:int}/reviews")]
    [ServiceFilter(typeof(ExistsMovieAttribute))]
    [ApiController]
    public class MoviesReviewsController : CustomControllerBase
    {
        private readonly IMovieReviewsServices movieReviewsServices;

        public MoviesReviewsController(ApplicationDbContext dbContext,
            IMapper mapper,
            IMovieReviewsServices movieReviewsServices)
            : base(dbContext, mapper)
        {
            this.movieReviewsServices = movieReviewsServices;
        }
        // GET: api/<MoviesReviewsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewReadingDto>>> Get(int movieId,
            [FromQuery] PaginationDto paginationDto)
        {
            IEnumerable<ReviewReadingDto> dtos = await this.movieReviewsServices.GetAllAsync(movieId,
                this.HttpContext, paginationDto);
            if (dtos != null && dtos.Any())
            {
                return Ok(dtos);
            }

            return NoContent();
        }

        // GET api/<MoviesReviewsController>/5
        [HttpGet("{id:int}", Name = "GetReviewById")]
        public async Task<ActionResult<ReviewReadingDto>> Get(int id)
        {
            ReviewReadingDto dto = await this.movieReviewsServices.GetByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }

            return NotFound();
        }

        // POST api/<MoviesReviewsController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ReviewReadingDto>> Post(int movieId, [FromBody] ReviewCreateUpdateDto dto)
        {
            ReviewReadingDto review = await this.movieReviewsServices.CreateAsync(movieId,
                this.HttpContext, dto);
            if (review != null)
            {
                return CreatedAtRoute("GetReviewById", new { MovieId = review.MovieId, Id = review.Id }, review);
            }

            return BadRequest();
        }

        // PUT api/<MoviesReviewsController>/5
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, [FromBody] ReviewCreateUpdateDto dto)
        {
            bool result = await this.movieReviewsServices.UpdateAsync(id, this.HttpContext, dto);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // DELETE api/<MoviesReviewsController>/5
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await this.movieReviewsServices.DeleteByIdAsync(id, this.HttpContext);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ReviewCreateUpdateDto> patchDocument)
        {
            return await this.Patch<Review, ReviewCreateUpdateDto>(id, patchDocument);
        }
    }
}