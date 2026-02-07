using Core.Exceptions;
using Core.FileSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Models.DTOs.FileSystem;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;

namespace Services.Collections
{
    public class UserUploads(IFileSystemProvider fileSystemProvider) : IValidatableObject, IUserUploads
    {
        private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
        private IFormFile _file;
        private string _userUri;

        public IUserUploads SetFile(IFormFile file)
        {
            this._file = file;
            return this;
        }

        public IUserUploads SetUserUri(string userUri)
        {
            this._userUri = userUri;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ArgumentsValidator.ThrowIfNull(nameof(this._file), this._file);
            ArgumentsValidator.ThrowIfNull(nameof(this._userUri), this._userUri);

            var results = new List<ValidationResult>();
            if (!this._fileSystemProvider.HasValidExtension(this._file))
            {
                results.Add(new ValidationResult($"Not valid extension. Only accept: {string.Join(", ", Enum.GetNames<UploadExtenstions>())}"));
            }

            if (!this._fileSystemProvider.HasValidSize(this._file))
            {
                results.Add(new ValidationResult($"File size should be less than: {FileSystemProvider.MAX_SIZE} Bytes"));
            }
            return results;
        }

        public IFormFile ProfileImage()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);

            string fileName = $"{this._userUri}.{this._fileSystemProvider.GetFileExtension(this._file)}";

            var filePath = Path.Combine(this._fileSystemProvider.GetProfileDirectory(), fileName);
            if (this._fileSystemProvider.FileExists(filePath))
            {
                if (!this._fileSystemProvider.TryDeleteFile(filePath))
                {
                    throw new ApiException(ApiExceptionCode.Forbidden, "unable to delete exisiting file", HttpStatusCode.Forbidden);
                }
            }

            return this._fileSystemProvider.Upload(this._file, filePath);
        }

        public ImageDTO GetUserProfileImage(string uri)
        {
            ArgumentsValidator.ThrowIfNull(nameof(uri), uri);



            var profileDir = this._fileSystemProvider.GetProfileDirectory();
            var filePath = Path.Combine(profileDir, uri);

            var files = this._fileSystemProvider.GetDuplicateFiles(filePath);

            if(files.Count() == 0) throw new ApiException(ApiExceptionCode.NotFound, "profile image not found", HttpStatusCode.NotFound);
            if (files.Count() > 1)
            {
                this._fileSystemProvider.TryDeleteFile(filePath);
                throw new ApiException(ApiExceptionCode.Conflict, "multiple image found. trying to clean all of them. please upload new image.", HttpStatusCode.Conflict);
            }

            var fileExtension = this._fileSystemProvider.GetFileExtension(files.FirstOrDefault()!);

            filePath = Path.ChangeExtension(filePath, fileExtension);

            byte[] bytes = File.ReadAllBytes(filePath);

            return new ImageDTO
            {
                Content = bytes,
                ContentType = $"image/{fileExtension}",
                FileName = Path.GetFileName(filePath)
            };
        }
    }
}
