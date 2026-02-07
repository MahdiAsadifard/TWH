using Microsoft.AspNetCore.Http;
using Models.DTOs.FileSystem;

namespace Services.Interfaces
{
    public interface IUserUploads
    {

        IUserUploads SetFile(IFormFile file);
        
        IUserUploads SetUserUri(string userUri);
        IFormFile ProfileImage();
        ImageDTO GetUserProfileImage(string uri);

    }
}