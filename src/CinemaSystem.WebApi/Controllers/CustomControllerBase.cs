using AutoMapper;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CinemaSystem.WebApi.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CustomControllerBase(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPatch("{id:int}")]
        protected async Task<ActionResult> Patch<TEntity, TDTO>(int id, [FromBody] JsonPatchDocument<TDTO> patchDocument)
            where TEntity : class, IId
            where TDTO : class
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            TEntity entity = await this.dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            TDTO dto = this.mapper.Map<TDTO>(entity);
            patchDocument.ApplyTo(dto, this.ModelState);
            bool isValid = TryValidateModel(dto);
            if (!isValid)
            {
                return BadRequest(this.ModelState);
            }

            this.mapper.Map(dto, entity);
            await this.dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
