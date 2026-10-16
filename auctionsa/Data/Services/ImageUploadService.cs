public class ImageUploadService
{
  private readonly IWebHostEnvironment _env;

  public ImageUploadService(IWebHostEnvironment env)
  {
    _env = env;
  }

  private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
  private const long MaxFileSize = 5 * 1024 * 1024; // Ngfaki limit ka 5MB

  private static string Sanitize(string input)
  {
    var cleaned = new string(input.Select(c => char.IsLetterOrDigit(c) || c == '-' || c == '_' ? c : '_').ToArray());
    return string.IsNullOrWhiteSpace(cleaned) ? "item" : cleaned;
  }

  public async Task<List<string>> SaveItemImagesAsync(string userEmail, string itemName, List<IFormFile> images)
  {
    var imagePaths = new List<string>();

    var safeUser = Sanitize(userEmail);
    var safeItem = Sanitize(itemName);

    var itemFolder = Path.Combine(_env.WebRootPath, "uploads", safeUser, safeItem);
    Directory.CreateDirectory(itemFolder);

    foreach (var image in images)
    {
      var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
      if (!AllowedExtensions.Contains(extension) || image.Length == 0 || image.Length > MaxFileSize)
      {
        continue;
      }

      var baseName = Sanitize(Path.GetFileNameWithoutExtension(image.FileName));
      var uniqueFileName = $"{baseName}_{Guid.NewGuid().ToString().Substring(0, 5)}{extension}";
      var filePath = Path.Combine(itemFolder, uniqueFileName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await image.CopyToAsync(fileStream);
      }

      imagePaths.Add($"uploads/{safeUser}/{safeItem}/{uniqueFileName}");
    }

    return imagePaths;
  }
}
