namespace Asp.netApp.Areas.Admin.Common
{
    public class FileUpload
    {
        public static async Task<string> UploadFile(IFormFile? uploadFile)
        {
            var uniqueFileName = "";
            if (uploadFile != null && uploadFile.Length > 0)
            {
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                var filePath = Path.Combine("wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

            }else
            {
                return null;
            }
            return uniqueFileName;
        }
    }
}
