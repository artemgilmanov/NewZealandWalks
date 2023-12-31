﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalksAPI.CustomActionFilters;
using NewZealandWalksAPI.Data;
using NewZealandWalksAPI.Models.Domain;
using NewZealandWalksAPI.Models.DTO;
using NewZealandWalksAPI.Repositories;
using System.Text.Json;

namespace NewZealandWalksAPI.Controllers
{
    // https://localhost:123/api/regions
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET ALL REGIONS
        // GET:https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> GetAll()
        {
            /* Loggind different levels
             *  logger.LogInformation("GetAllRegions Action Mehtod was invoked.");
             *  logger.LogWarning("This is a warning log.");
             *  logger.LogError("This is a error log.");
             */

            ///* Logging an Exception
            try
            {
                //throw new Exception("This is a custom exception.");

                //Get Data From Database - Domain Models
                var regionsDomain = await regionRepository.GetAllAsync();

                logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

                // Return DTOs back to Client
                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            //*/

            /*
            //Get Data From Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();

            //logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

            // Return DTOs back to Client
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            */

        }

        // GET SINGLE REGION (Get Region By Id)
        // GET:https://localhost:portnumber/api/regionsDomain/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            //var regionDomain = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Return DTOs back to Client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST to Create New Region
        // POST:https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        // PUT: htpps:/localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete Region
        // DELETE: htpp://localhost: portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
