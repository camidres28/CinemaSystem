using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Cinemas;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.CinemaServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/cinemas")]
    [ApiController]
    public class CinemasController : CustomControllerBase
    {
        private readonly ICinemaServices cinemaServices;

        public CinemasController(ApplicationDbContext dbContext, IMapper mapper,
            ICinemaServices cinemaServices)
            : base(dbContext, mapper)
        {
            this.cinemaServices = cinemaServices;
        }
        // GET: api/<CinemasController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaDto>>> Get([FromQuery] PaginationDto paginationDto)
        {
            IEnumerable<CinemaDto> dtos = await this.cinemaServices.GetAllAsync(this.HttpContext, paginationDto);
            if (dtos != null)
            {
                return Ok(dtos);
            }

            return NoContent();
        }
        [HttpGet("nearby")]
        public async Task<ActionResult<IEnumerable<CinemaNearbyDto>>> Get([FromQuery] CinemaNearbyFilterDto dto)
        {
            IEnumerable<CinemaNearbyDto> cinemas = await this.cinemaServices.GetNearbyAsync(dto);
            if (cinemas != null)
            {
                return Ok(cinemas);
            }

            return NoContent();
        }

        // GET api/<CinemasController>/5
        [HttpGet("{id:int}", Name = "GetCinemaById")]
        public async Task<ActionResult<CinemaDetailsDto>> Get(int id)
        {
            CinemaDetailsDto dto = await this.cinemaServices.GetByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }

            return NotFound();
        }

        // POST api/<CinemasController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CinemaCreationUpdateDto dto)
        {
            CinemaDto cinemaDto = await this.cinemaServices.CreateAsync(dto);
            if (cinemaDto != null)
            {
                return CreatedAtRoute("GetCinemaById", new { Id = cinemaDto.Id }, cinemaDto);
            }

            return NoContent();
        }

        // PUT api/<CinemasController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CinemaCreationUpdateDto dto)
        {
            bool result = await this.cinemaServices.UpdateAsync(id, dto);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<CinemasController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool result = await this.cinemaServices.DeleteByIdAsync(id);
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
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CinemaBaseDto> patchDocument)
        {
            ActionResult result = await this.Patch<Cinema, CinemaBaseDto>(id, patchDocument);

            return result;
        }
    }
}
