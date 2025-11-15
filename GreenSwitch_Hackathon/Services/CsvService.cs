using GreenSwitch.Models;

namespace GreenSwitch.Services
{
    public interface ICsvService
    {
        List<Supplier> ReadSuppliers();
        List<Supplier> SearchSuppliers(string? category = null, string? location = null);
    }

    public class CsvService : ICsvService
    {
        private readonly IWebHostEnvironment _environment;

        public CsvService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public List<Supplier> ReadSuppliers()
        {
            var suppliers = new List<Supplier>();
            var csvPath = Path.Combine(_environment.ContentRootPath, "Data", "suppliers.csv");

            if (!File.Exists(csvPath))
            {
                CreateSampleCsv(csvPath);
            }

            using var reader = new StreamReader(csvPath);
            reader.ReadLine(); // Skip header

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');
                if (values.Length >= 8)
                {
                    suppliers.Add(new Supplier
                    {

                        Name = values[0],
                        Category = values[1],
                        Location = values[2],
                        SDGs = values[3].Split(';').ToList(),
                        Services = values[4],
                        Specialty = values[5],
                        Rating = decimal.Parse(values[6]),
                        Contact = values[7],
                        Status = Enum.Parse<VerificationStatus>(values[8]),
                        VerifiedDate = DateTime.Parse(values[9]),
                        Certifications = values[10].Split(';').ToList(),
                        VerificationNotes = values[11]
                    
                });
                }
            }

            return suppliers;
        }

        public List<Supplier> SearchSuppliers(string? category = null, string? location = null)
        {
            var allSuppliers = ReadSuppliers();

            return allSuppliers.Where(s =>
                (category == null || s.Category.Contains(category, StringComparison.OrdinalIgnoreCase)) &&
                (location == null || s.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        private void CreateSampleCsv(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            var sampleData = @"Name,Category,Location,SDGs,Services,Specialty,Rating,Contact
Mauritius Solar Solutions,Solar,Port Louis,9;13,Installation;Maintenance,Commercial Solar,4.8,contact@mss.mu
SunPower Mauritius,Solar,Curepipe,9;13,Residential;Battery Storage,Home Solutions,4.6,info@sunpower.mu
EcoPack Mauritius,Packaging,Curepipe,9;14,Biodegradable;Compostable,Food Packaging,4.9,hello@ecopack.mu
Organic Farm Co-op,Agriculture,Rose Belle,9;13,Produce;Delivery,Local Organic,4.7,farm@organic.mu
Blue Ocean Initiative,Conservation,Grand Baie,13;14,Cleanups;Education,Marine Protection,4.5,contact@blueocean.mu
GreenWrap Solutions,Packaging,Port Louis,9;14,Recycled Paper;Custom,Retail Packaging,4.7,sales@greenwrap.mu";

            File.WriteAllText(path, sampleData);
        }
    }
}