using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.DTO;
using API.Entities;

namespace API.Services
{
    public interface IExaminationCodeService
    {

        Task<IActionResult> GetSuggestionsAsync(ExaminationType examinationType);

        Task<IActionResult> CreateExaminationCode (ExaminationCodeDto examinationCodeDto);
    }
}