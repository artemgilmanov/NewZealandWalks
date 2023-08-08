using NewZealandWalksAPI.Models.Domain;

namespace NewZealandWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
    }
}
