public class ImageUploadService
{
  private readonly IWebHostEnvironment _env;

  public ImageUploadService(IWebHostEnvironment env)
  {
    _env = env;
  }

  public async Task<List<string>> SaveItemImagesAsync(string userEmail, string itemName, List<IFormFile> images)
  {
    var imagePaths = new List<string>();

    // Step 1: Define user folder path
    var userFolder = Path.Combine(_env.WebRootPath, "uploads", userEmail);

    // Check if user's folder exists, create if not
    if (!Directory.Exists(userFolder))
    {
      Directory.CreateDirectory(userFolder);
    }

    // Step 2: Create a subfolder for the item under the user's folder
    var itemFolder = Path.Combine(userFolder, itemName);
    if (!Directory.Exists(itemFolder))
    {
      Directory.CreateDirectory(itemFolder);
    }

    // Step 3: Save each image file in the item folder
    foreach (var image in images)
    {
      var extension = Path.GetExtension(image.FileName);
      var uniqueFileName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{Guid.NewGuid().ToString().Substring(0, 5)}{extension}";
      var filePath = Path.Combine(itemFolder, uniqueFileName);

      using (var fileStream = new FileStream(filePath, FileMode.Create))
      {
        await image.CopyToAsync(fileStream);
      }

      var relativePath = Path.Combine("uploads", userEmail, itemName, uniqueFileName);
      imagePaths.Add(relativePath.Replace("\\", "/"));
    }

    return imagePaths;
  }
}
