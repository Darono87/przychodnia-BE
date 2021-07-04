using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IExaminationCodeService
    {

        Task<IActionResult> GetSuggestionsAsync();
    }
}