using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class ExaminationCodeService : IExaminationCodeService
    {
        private readonly IExaminationCodeRepository examinationCodeRepository;

        public ExaminationCodeService(IExaminationCodeRepository examinationCodeRepository)
        {
            this.examinationCodeRepository = examinationCodeRepository;
        }

        public async Task<IActionResult> GetSuggestionsAsync(){
            var suggestions = await examinationCodeRepository.GetAllAsync();
            return new JsonResult(suggestions);
        }
    }
}
