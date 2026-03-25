using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ParaBank_Automation.Utilities
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Đọc file JSON dạng Array → List<T>
        public static List<T> ReadTestData<T>(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file test data: {fullPath}");
            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }

        // Đọc file JSON dạng Object → T
        public static T ReadTestDataObject<T>(string filePath) where T : new()
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file test data: {fullPath}");
            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<T>(json, Options) ?? new T();
        }
    }
}
