﻿using Microsoft.AspNetCore.Mvc;
using NewZealandWalksAPI.Data;
using NewZealandWalksAPI.Models.Domain;
using NewZealandWalksAPI.Models.DTO;

namespace NewZealandWalksAPI.Controllers
{
    // https://localhost:123/api/regionsDomain
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET ALL REGIONS
        // GET:https://localhost:portnumber/api/regionsDomain
        [HttpGet]
        public IActionResult GetAll()
        {
            //Get Data From Database - Domain Models
            var regionsDomain = dbContext.Regions.ToList();

            // Map Domain Models to DTOs
            var regionsDTO = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain) 
            {
                regionsDTO.Add(new RegionDto()
                {
                    Id= regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl= regionDomain.RegionImageUrl
                });
            }

            // Return DTOs back to Client
            return Ok(regionsDTO);
        }

        // GET SINGLE REGION (Get Region By Id)
        // GET:https://localhost:portnumber/api/regionsDomain/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //var regionDomain = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            //
            var regionDTO = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // Return DTOs back to Client
            return Ok(regionDomain);
        }

        // POST to Create New Region
        // POST:https://localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        // PUT: htpps:/localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            // Check if region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl= updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            //Convert Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Delete Region
        // DELETE: htpp://localhost: portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Delete region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            // return deleted Region back
            // map Domain Model to DTO
            var regionsDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionsDto);
        }


    }
}
