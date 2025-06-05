using HotChocolate.Types;

namespace Application.Utils
{
    public static class FileHandler
    {
        public static async Task<string> StoreImage(string entity, IFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Image file is null or empty.");
            }

            var uploadsFolder = Path.Combine("wwwroot", "uploads", entity);
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{file.Name}");
            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);

            return filePath.Replace("wwwroot", string.Empty).Replace("\\", "/");
        }

        public static async Task<bool> DeleteImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}
