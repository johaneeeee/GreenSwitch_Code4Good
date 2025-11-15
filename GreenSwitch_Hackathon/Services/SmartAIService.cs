using GreenSwitch.Models;

namespace GreenSwitch.Services
{
    public interface ISmartAIService
    {
        Task<ChatResponse> ProcessMessageAsync(string userMessage);
    }

    public class SmartAIService : ISmartAIService
    {
        private readonly ICsvService _csvService;

        public SmartAIService(ICsvService csvService)
        {
            _csvService = csvService;
        }

        public async Task<ChatResponse> ProcessMessageAsync(string userMessage)
        {
            await Task.Delay(300); // Simulate AI processing

            // Step 1: Analyze intent
            var intent = AnalyzeIntent(userMessage);

            // Step 2: Get relevant suppliers
            var suppliers = GetRelevantSuppliers(userMessage, intent);

            // Step 3: Generate smart response
            var response = GenerateResponse(userMessage, intent, suppliers);

            return new ChatResponse
            {
                Success = true,
                Response = response,
                Suppliers = suppliers.Take(3).ToList(),
                Intent = intent
            };
        }

        private string AnalyzeIntent(string message)
        {
            var lower = message.ToLower();

            if (lower.Contains("solar") || lower.Contains("energy")) return "solar";
            if (lower.Contains("packaging") || lower.Contains("plastic")) return "packaging";
            if (lower.Contains("organic") || lower.Contains("farm")) return "agriculture";
            if (lower.Contains("sdg") || lower.Contains("sustainable")) return "sdg";
            if (lower.Contains("restaurant") || lower.Contains("hotel")) return "hospitality";
            if (lower.Contains("clean") || lower.Contains("recycl")) return "conservation";

            return "general";
        }

        private List<Supplier> GetRelevantSuppliers(string message, string intent)
        {
            var suppliers = _csvService.ReadSuppliers();
            var lower = message.ToLower();

            return intent switch
            {
                "solar" => suppliers.Where(s => s.Category.Equals("Solar", StringComparison.OrdinalIgnoreCase)).ToList(),
                "packaging" => suppliers.Where(s => s.Category.Equals("Packaging", StringComparison.OrdinalIgnoreCase)).ToList(),
                "agriculture" => suppliers.Where(s => s.Category.Equals("Agriculture", StringComparison.OrdinalIgnoreCase)).ToList(),
                "conservation" => suppliers.Where(s => s.Category.Equals("Conservation", StringComparison.OrdinalIgnoreCase)).ToList(),
                _ => suppliers.Take(2).ToList()
            };
        }

        private string GenerateResponse(string userMessage, string intent, List<Supplier> suppliers)
        {
            return intent switch
            {
                "solar" => GenerateSolarResponse(suppliers),
                "packaging" => GeneratePackagingResponse(suppliers),
                "agriculture" => GenerateAgricultureResponse(suppliers),
                "sdg" => GenerateSdgResponse(),
                "hospitality" => GenerateHospitalityResponse(suppliers),
                "conservation" => GenerateConservationResponse(suppliers),
                _ => GenerateGeneralResponse(suppliers)
            };
        }

        private string GenerateSolarResponse(List<Supplier> suppliers)
        {
            if (!suppliers.Any())
                return "I don't have solar suppliers in my database yet. Try asking about packaging or agriculture!";

            var response = "Solar Energy Solutions in Mauritius:\n\n";

            foreach (var supplier in suppliers.Take(3))
            {
                response += $"{supplier.Name} - {supplier.Location}\n";
                response += $" {supplier.Rating}/5 | {supplier.Services.Replace(";", ", ")}\n";
                response += $" SDG Impact: {string.Join(", ", supplier.SDGs.Select(sdg => $"SDG {sdg}"))}\n";
                response += $"  {supplier.Specialty}\n";
                response += $"  {supplier.Contact}\n\n";
            }

            response += "💡 Sustainable Impact: Solar energy reduces carbon emissions and supports Mauritius' renewable energy goals!";
            return response;
        }

        private string GeneratePackagingResponse(List<Supplier> suppliers)
        {
            if (!suppliers.Any())
                return "I don't have packaging suppliers in my database yet. Check back soon for updates!";

            var response = "Eco-Friendly Packaging Solutions:\n\n";

            foreach (var supplier in suppliers.Take(3))
            {
                response += $"{supplier.Name} - {supplier.Location}\n";
                response += $"{supplier.Rating}/5 | {supplier.Services.Replace(";", ", ")}\n";
                response += $"Specializes in: {supplier.Specialty}\n";
                response += $"{supplier.Contact}\n\n";
            }

            response += "🌊 SDG 14 Impact: These suppliers help reduce ocean plastic pollution in Mauritius!";
            return response;
        }

        private string GenerateSdgResponse()
        {
            return @" Sustainable Development Goals for Mauritius:

                           SDG 9 - Industry, Innovation & Infrastructure
                            • Renewable energy systems
                            • Sustainable business practices

                           SDG 13 - Climate Action
                            • Carbon footprint reduction
                            • Climate resilience planning

                          SDG 14 - Life Below Water
                            • Plastic waste reduction
                            • Coastal ecosystem protection

                    🌍 The Connection: Sustainable infrastructure enables climate action that protects our oceans!";
        }

        private string GenerateGeneralResponse(List<Supplier> suppliers)
        {
            var categories = suppliers.Select(s => s.Category).Distinct();

            return $@"Welcome to your chat assistant!

                      I can help you find sustainable suppliers in Mauritius. Try asking about:<br>

                       • Solar energy solutions
                       • Eco-friendly packaging
                       • Organic agriculture
                       • SDG guidance

                      I have {suppliers.Count} verified suppliers ready to help your business go green!";
        }

        private string GenerateAgricultureResponse(List<Supplier> suppliers)
        {
            if (!suppliers.Any()) return "No agriculture suppliers found.";

            var response = "Organic Agriculture in Mauritius:\n\n";
            foreach (var supplier in suppliers.Take(3))
            {
                response += $"{supplier.Name} - {supplier.Location}\n";
                response += $"{supplier.Rating}/5 | {supplier.Services}\n";
                response += $" {supplier.Contact}\n\n";
            }
            return response;
        }

        private string GenerateHospitalityResponse(List<Supplier> suppliers)
        {
            return @"🏨 Sustainable Practices for Hospitality:

For restaurants and hotels in Mauritius, I recommend:

1. Solar energy - Reduce electricity costs
2. Eco-packaging - Eliminate single-use plastics  
3. Local organic - Source from Mauritian farms
4. Waste management - Implement recycling systems

Ask me about specific suppliers in any category!";
        }

        private string GenerateConservationResponse(List<Supplier> suppliers)
        {
            if (!suppliers.Any()) return "No conservation suppliers found.";

            var response = "Conservation & Environmental Services:\n\n";
            foreach (var supplier in suppliers.Take(3))
            {
                response += $"{supplier.Name} - {supplier.Location}\n";
                response += $"{supplier.Rating}/5 | {supplier.Services}\n";
                response += $"{supplier.Contact}\n\n";
            }
            return response;
        }
    }
}