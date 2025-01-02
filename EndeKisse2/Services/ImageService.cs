using Supabase.Storage;
using Supabase;

namespace EndeKisse2.Services
{

    public class ImageService
    {
        private readonly Supabase.Client _supabaseClient;

        public ImageService(SupabaseService supabaseService)
        {
            _supabaseClient = supabaseService.GetClient();
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string bucketName)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid file.");

            var storage = _supabaseClient.Storage;
            var bucket = storage.From(bucketName);

            string fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                await bucket.Upload(memoryStream.ToArray(), fileName);
            }

            // Get the public URL
            string publicUrl = bucket.GetPublicUrl(fileName);
            return publicUrl;
        }
    }

    
}
