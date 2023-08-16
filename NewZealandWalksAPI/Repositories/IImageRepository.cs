using NewZealandWalksAPI.Models.Domain;
using System.Net;

namespace NewZealandWalksAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
