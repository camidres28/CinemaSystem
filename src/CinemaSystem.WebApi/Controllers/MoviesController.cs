using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MovieServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieServices movieServices;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public MoviesController(IMovieServices movieServices,
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.movieServices = movieServices;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> Get([FromQuery] PaginationDto paginationDto)
        {
            IEnumerable<MovieDto> dtos = await this.movieServices.GetAllAsync(this.HttpContext, paginationDto);
            if (dtos.Any())
            {
                return Ok(dtos);
            }

            return NoContent();
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id:int}", Name ="GetMovieById")]
        public async Task<ActionResult<MovieDto>> Get(int id)
        {
            MovieDto dto = await this.movieServices.GetByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }

            return NotFound();
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreateUpdateDto dto)
        {
            MovieDto movieDto = await this.movieServices.CreateAsync(dto);
            if (movieDto != null)
            {
                ActionResult result = CreatedAtRoute("GetMovieById", new { Id = movieDto.Id }, movieDto);
                return result;
            }

            return NoContent();
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreateUpdateDto dto)
        {
            await this.movieServices.UpdateAsync(id, dto);

            return NoContent();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this.movieServices.DeleteByIdAsync(id);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MovieBaseDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            Movie entity = await this.dbContext.Movies.FirstOrDefaultAsync(x=>x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            MovieBaseDto movieBaseDto = this.mapper.Map<MovieBaseDto>(entity);
            patchDocument.ApplyTo(movieBaseDto, this.ModelState);
            bool isValid = TryValidateModel(movieBaseDto);
            if (!isValid)
            {
                return BadRequest(this.ModelState);
            }

            this.mapper.Map(movieBaseDto, entity);
            await this.dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
