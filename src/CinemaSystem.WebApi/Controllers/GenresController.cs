﻿using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.GenreServices;
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
    [Route("api/genres")]
    [ApiController]
    public class GenresController : CustomControllerBase
    {
        private readonly IGenreServices genreServices;

        public GenresController(IGenreServices genreServices,
            ApplicationDbContext dbContext,
            IMapper mapper)
            : base(dbContext, mapper)
        {
            this.genreServices = genreServices;
        }

        // GET: api/<GenresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get([FromQuery] PaginationDto paginationDto)
        {
            IEnumerable<GenreDto> dtos = await this.genreServices.GetAllAsync(this.HttpContext, paginationDto);
            if (dtos.Any())
            {
                return Ok(dtos);
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
            bool result = await this.genreServices.UpdateAsync(id, dto);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await this.genreServices.DeleteByIdAsync(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<GenreCreateUpdateDto> patchDocument)
        {
            ActionResult result = await this.Patch<Genre, GenreCreateUpdateDto>(id, patchDocument);

            return result;
        }
    }
}
