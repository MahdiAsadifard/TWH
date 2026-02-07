using Microsoft.AspNetCore.Http;

namespace Core.FileSystem
{
    public interface IFileSystemProvider
    {
        bool FileExists(string filePath);
        string GetFileExtension(IFormFile file);
        string GetProfileDirectory();
        string GetUploadDirectory(FileSystemPaths paths);
        string GetFileExtension(string filePath);
        bool HasValidExtension(IFormFile file);
        bool HasValidSize(IFormFile file);
        IEnumerable<string> GetDuplicateFiles(string filePath);
        bool TryDeleteFile(string filePath);
        IFormFile Upload(IFormFile file, string fullPath);
    }
}