﻿namespace NewZealandWalksAPI.Models.DTO
{
    public class AddWalkRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DificultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
