using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Services.GenreServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreServices genreServices;

        public GenresController(IGenreServices genreServices)
        {
            this.genreServices = genreServices;
        }

        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            IEnumerable<GenreDto> genres = await this.genreServices.GetAllAsync();
            if (genres.Any())
            {
                return Ok(genres);
            }

            return NoContent();
        }

        // GET api/<GenresController>/5
        [HttpGet("{id:int}", Name = "GetGenreById")]
        public async Task<ActionResult<GenreDto>> Get(int id)
        {
            GenreDto dto = await this.genreServices.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }

            return dto;
        }

        // POST api/<GenresController>
        [HttpPost]
        public async Task<ActionResult<GenreDto>> Post([FromBody] GenreCreateUpdateDto dto)
        {
            GenreDto genreDto = await this.genreServices.CreateAsync(dto);

            //return CreatedAtAction("GET", new { Id = genreDto.Id }, genreDto);
            return CreatedAtRoute("GetGenreById", new { Id = genreDto.Id }, genreDto);
        }

        // PUT api/<GenresController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreateUpdateDto dto)
        {
            await this.genreServices.UpdateAsync(id, dto);

            return NoContent();
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this.genreServices.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
