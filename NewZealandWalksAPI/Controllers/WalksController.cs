using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalksAPI.CustomActionFilters;
using NewZealandWalksAPI.Models.Domain;
using NewZealandWalksAPI.Models.DTO;
using NewZealandWalksAPI.Repositories;

namespace NewZealandWalksAPI.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepositiory;

        public WalksController(IMapper mapper, IWalkRepository walkRepositiory)
        {
            this.mapper = mapper;
            this.walkRepositiory = walkRepositiory;
        }
        // Create walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to DomainModel
            var walkDomainlModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepositiory.CreateAsync(walkDomainlModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainlModel));
        }

        // Get Walks
        // GET: /api/walks?folterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filteredOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepositiory.GetAllAsync(filteredOn, filterQuery,
                sortBy, isAscending ?? true,
                pageNumber, pageSize);

            // Create an exseption
            throw new Exception("This is a new exception.");

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel)) ;
        }

        // Get Walk by Id
        // GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepositiory.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map DomainModel to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update walk by id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepositiory.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
        }

        // Delete a walk by id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepositiory.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
