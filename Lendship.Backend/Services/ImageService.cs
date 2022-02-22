using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
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
        private readonly LendshipDbContext _dbContext;

        private static Random random = new Random();

        public ImageService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public void DeleteProfileImage()
        {
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "Resources/Images");
            DeleteFiles(imgLocation);
        }

        public void DeleteImages(int advertisementId)
        {
            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "Resources/Images");
            DeleteFiles(imgLocation, advertisementId);
        }

        public void DeleteImageFromAdvertisement(int advertisementId, string fileName)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var location = _configuration.GetSection("Image").GetValue("LocationFolder", "Resources/Images");
            var directory = Path.Combine(location, signedInUserId, advertisementId.ToString(), fileName);

            File.Delete(directory);
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
        }

        public List<ImageDTO> GetImages(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "Resources/Images");
            var directory = Path.Combine(imgLocation, signedInUserId, advertisementId.ToString());

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
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "Resources/Images");

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
            var imgLocation = _configuration.GetSection("Image").GetValue("ProfileLocationFolder", "Resources/Images");

            var path = Path.Combine(imgLocation, signedInUserId);

            UploadImg(file, path);
        }

        public void UploadImages(IFormFileCollection files, int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ad = _dbContext.Advertisements
                .Include(a => a.User)
                .Where(a => a.Id == advertisementId)
                .FirstOrDefault();

            if(ad.User.Id != signedInUserId)
            {
                throw new UpdateNotAllowedException("The advertisement does not belong to this user.");
            }

            var imgLocation = _configuration.GetSection("Image").GetValue("LocationFolder", "Resources/Images");

            foreach (var file in files)
            {
                CheckFileFormat(file.ContentType);

                var path = Path.Combine(imgLocation, signedInUserId, advertisementId.ToString());

                UploadImg(file, path);
            }
        }

        private void UploadImg(IFormFile file, string path)
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
