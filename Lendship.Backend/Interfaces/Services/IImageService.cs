using Lendship.Backend.DTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IImageService
    {
        string UploadProfileImage(IFormFile file);

        void UploadImages(IFormFileCollection files, int advertisementId);

        void DeleteProfileImage();

        void DeleteImages(int advertisementId);

        void DeleteImageFromAdvertisement(int advertisementId, string fileName);

        List<ImageDTO> GetImages(int advertisementId);

        ImageDTO GetImage();
    }
}
