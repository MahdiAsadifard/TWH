using Microsoft.AspNetCore.Http;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.FileSystem
{
    public class FileSystemProvider : IFileSystemProvider
    {
        public const int MAX_SIZE = 5 * 1024 * 1024; // 5 MB


        public FileSystemProvider()
        {
            this.CreateCustomDirectories();
        }

        private void CreateCustomDirectories()
        {
            var assetsDir = GetUploadDirectory(FileSystemPaths.Assets);

            // Create Assets directory if it doesn't exist
            Directory.CreateDirectory(assetsDir);

            Enum
                .GetValues<FileSystemPaths>().
                ToList()
                .ForEach(path =>
                {
                    if (path != FileSystemPaths.Assets)
                    {
                        Directory.CreateDirectory(Path.Combine(assetsDir, path.ToString()));
                    }
                });

        }

        public string GetUploadDirectory(FileSystemPaths paths)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectDir = new DirectoryInfo(currentDir).Parent.Parent;

            return Path.Combine(projectDir!.ToString(), paths.ToString());
        }

        public string GetProfileDirectory()
        {
            return Path.Combine(GetUploadDirectory(FileSystemPaths.Assets), nameof(FileSystemPaths.Profile));
        }

        public string GetFileExtension(IFormFile file)
        {
            return Path
                .GetExtension(file.FileName)
                .TrimStart('.')
                .ToLowerInvariant();
        }
        public string GetFileExtension(string filePath)
        {
            return Path
                .GetExtension(filePath)
                .TrimStart('.')
                .ToLowerInvariant();
        }

        public bool FileExists(string filePath)
        {
            return GetDuplicateFiles(filePath).Count() > 0;
        }

        public IEnumerable<string> GetDuplicateFiles(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            DirectoryInfo d = new DirectoryInfo(filePath);
            var fileNameWithoutExtension = d.Name.Remove(d.Name.IndexOf(fileExtension), fileExtension.Length);

            string dir = Path.GetDirectoryName(filePath)!;

            return Directory
                .GetFiles(dir)
                .Where(x => x.Contains(Path.Combine(dir, fileNameWithoutExtension + "." )));
        }

        public bool TryDeleteFile(string filePath)
        {
            try
            {
                foreach (var item in GetDuplicateFiles(filePath))
                {
                    File.Delete(item);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool HasValidExtension(IFormFile file)
        {
            string[] names = Enum.GetNames<UploadExtenstions>();
            return names.Contains(GetFileExtension(file));
        }

        public bool HasValidSize(IFormFile file)
        {
            return file.Length < MAX_SIZE;
        }
        public IFormFile Upload(IFormFile file, string fullPath)
        {
            using Stream stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return file;
        }
    }
}
