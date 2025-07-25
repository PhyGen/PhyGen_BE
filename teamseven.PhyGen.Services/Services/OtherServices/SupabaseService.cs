using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Supabase;
using Supabase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace teamseven.PhyGen.Services.Services.OtherServices
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(IConfiguration config)
        {
            var url = config["Supabase:Url"];
            var key = config["Supabase:Key"];

            _client = new Supabase.Client(url, key);
            _client.InitializeAsync().Wait();
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var bucket = _client.Storage.From("blobs");

            var options = new Supabase.Storage.FileOptions
            {
                CacheControl = "3600",
                Upsert = true
            };

            // ✅ đúng hàm, đúng kiểu
            var response = await bucket.Upload(fileBytes, fileName, options);

            // ✅ lấy URL công khai
            var publicUrl = bucket.GetPublicUrl(fileName);

            return publicUrl;
        }


    }
}
