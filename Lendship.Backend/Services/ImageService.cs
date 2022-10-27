using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Lendship.Backend.Services
{
    public class ImageService : IImageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IImageLocationRepository _imageRepository;

        private static Random random = new Random();

        public ImageService(
            IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration, 
            IUserRepository userRepository,
            IAdvertisementRepository advertisementRepository,
            IImageLocationRepository imageRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _imageRepository = imageRepository;
        }

        public void DeleteProfileImage()
        {
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "wwwroot\\images\\profile");
            DeleteFiles(imgLocation);

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetById(signedInUserId);
            user.ImageLocation = "";
            _userRepository.Update(user);

        }

        public void DeleteImages(int advertisementId)
        {
            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "wwwroot\\images");
            DeleteFiles(imgLocation, advertisementId);
        }

        public void DeleteImageFromAdvertisement(int advertisementId, string fileName)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var location = _configuration.GetSection("Image").GetValue("LocationFolder", "wwwroot\\images");
            var directory = Path.Combine(location, signedInUserId, advertisementId.ToString(), fileName);

            File.Delete(directory);
            _imageRepository.DeleteImageFromAdvertisement(advertisementId, fileName);
        }

        private void DeleteFiles(string location, int? advertisementId = null)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var directory = Path.Combine(location, signedInUserId, advertisementId.ToString());

            try
            {
                var paths = Directory.GetFiles(directory);

                foreach (var path in paths)
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No file in the directory." + e.Message);
            }

            _imageRepository.DeleteImagesByAdvertisement(advertisementId);
        }

        public List<ImageDTO> GetImages(int advertisementId)
        {
            var userId = _advertisementRepository.GetById(advertisementId).User.Id;
            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "wwwroot\\images");
            var directory = Path.Combine(imgLocation, userId, advertisementId.ToString());

            List<ImageDTO> result = new List<ImageDTO>();

            try
            {
                var paths = Directory.GetFiles(directory);

                foreach (var path in paths)
                {
                    var img = ReadFile(path);
                    result.Add(img);
                }
            } catch (Exception e)
            {
                Console.WriteLine("No file in the directory." + e.Message);
            }
            return result;
        }

        public ImageDTO GetImage()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "wwwroot\\images\\profile");

            var directory = Path.Combine(imgLocation, signedInUserId);

            try
            {
                var path = Directory.GetFiles(directory).First();
                return ReadFile(path);
            }
            catch (Exception)
            {
                throw new NoFileException("There isn't any images for this advertisement.");
            }
        }

        private ImageDTO ReadFile(string path)
        {
            var bytes = File.ReadAllBytes(path);

            return new ImageDTO
            {
                Name = path.Substring(path.LastIndexOf('\\') + 1),
                Bytes = Convert.ToBase64String(bytes, 0, bytes.Length)
            };
        }

        public void UploadProfileImage(IFormFile file)
        {
            if (file.ContentType != "image/jpg" && file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            {
                throw new BadFileFormatException("Bad file format, we except jpg, jpeg, png.");
            }

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetById(signedInUserId);
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "wwwroot\\images\\profile");

            var path = Path.Combine(imgLocation, signedInUserId);

            var fullpath = UploadProfileImg(file, path);
            user.ImageLocation = fullpath;

            _userRepository.Update(user);
        }

        public void UploadImages(IFormFileCollection files, int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ad = _advertisementRepository.GetById(advertisementId);

            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "wwwroot\\images");

            if (ad.User.Id != signedInUserId)
            {
                throw new UpdateNotAllowedException("The advertisement does not belong to this user.");
            }

            foreach (var file in files)
            {
                CheckFileFormat(file.ContentType);

                var fullPath = Path.Combine(imgLocation, signedInUserId, advertisementId.ToString());
                var path = Path.Combine("\\", signedInUserId, advertisementId.ToString());

                UploadImg(file, fullPath, path, advertisementId);
            }

            _advertisementRepository.Update(ad);
        }

        private string UploadProfileImg(IFormFile file, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = GetUniqueFileName(path, file.ContentType);

            var fullPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fullPath;
        }

        private void UploadImg(IFormFile file, string fullPath, string path, int advertisementId)
        {

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var fileName = GetUniqueFileName(fullPath, file.ContentType);

            var fullPathWidthName = Path.Combine(fullPath, fileName);

            using (var stream = new FileStream(fullPathWidthName, FileMode.Create))
            {
                file.CopyTo(stream);
                _imageRepository.Create(advertisementId, Path.Combine(path, fileName));
            }
        }

        private string GetUniqueFileName(string path, string contentType)
        {
            var existingNames = Directory.GetFiles(path);
            string name;

            do
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                name = new string(Enumerable.Repeat(chars, 16)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (existingNames.Contains(Path.Combine(path, name)));

            var nameBuilder = new StringBuilder();
            nameBuilder.Append(name);
            nameBuilder.Append('.');
            nameBuilder.Append(contentType.Substring(contentType.LastIndexOf('/') + 1));

            return nameBuilder.ToString();
        }

        private void CheckFileFormat(string contentType)
        {
            if (contentType != "image/jpg" && contentType != "image/jpeg" && contentType != "image/png")
            {
                throw new BadFileFormatException("Bad file format, we except jpg, jpeg, png.");
            }
        }
    }
}
