using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Services.ActorServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorServices actorServices;

        public ActorsController(IActorServices actorServices)
        {
            this.actorServices = actorServices;
        }

        // GET: api/<ActorsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> Get()
        {
            IEnumerable<ActorDto> dtos = await this.actorServices.GetAllAsync();
            if (dtos.Any())
            {
                return Ok(dtos);
            }

            return NoContent();
        }

        // GET api/<ActorsController>/5
        [HttpGet("{id:int}", Name = "GetActorById")]
        public async Task<ActionResult<ActorDto>> Get(int id)
        {
            ActorDto dto = await this.actorServices.GetByIdAsync(id);
            if (dto != null)
            {
                return Ok(dto);
            }

            return NotFound();
        }

        // POST api/<ActorsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreateUpdateDto dto)
        {
            ActorDto actorDto = await this.actorServices.CreateAsync(dto);
            if (actorDto != null)
            {
                return CreatedAtRoute("GetActorById", new { Id = actorDto.Id }, actorDto);
            }

            return NoContent();
        }

        // PUT api/<ActorsController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreateUpdateDto dto)
        {
            await this.actorServices.UpdateAsync(id, dto);

            return NoContent();
        }

        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await this.actorServices.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
