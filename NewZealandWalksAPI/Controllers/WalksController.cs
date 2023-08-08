using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to DomainModel
            var walkDomainlModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepositiory.CreateAsync(walkDomainlModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainlModel));

        }
    }
}
