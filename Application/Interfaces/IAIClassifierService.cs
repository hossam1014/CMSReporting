using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.Interfaces
{
    public interface IAIClassifierService
    {
        Task<Result<ClassificationResult>> ClassifyTextAsync(string text);
    }

    public class ClassificationResult
    {
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Subcategory { get; set; }
    }
} 