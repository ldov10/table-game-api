using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models.Entities;

namespace UserService.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task SaveImageAsync(Image image);

        Task RemoveUserImagesAsync(long userId);

        Task<List<Image>> GetUserImagesAsync(long userId);
    }
}
