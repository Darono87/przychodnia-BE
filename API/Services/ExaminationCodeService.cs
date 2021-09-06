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

        public async Task<IActionResult> GetSuggestionsAsync(ExaminationType examinationType){
            var suggestions = await examinationCodeRepository.GetAllAsync(examinationType);
            return new JsonResult(suggestions);
        }

        public async Task<IActionResult> CreateExaminationCode (ExaminationCodeDto examinationCodeDto){
            if (examinationCodeDto.Abbreviation == "")
            {
                throw new ArgumentException("Abbreviation cannot be empty");
            }
            if (examinationCodeDto.Name == "")
            {
                throw new ArgumentException("Name cannot be empty");
            }

            var examinationCode = new ExaminationCode(){
                Abbreviation = examinationCodeDto.Abbreviation,
                Name = examinationCodeDto.Name,
                Type = examinationCodeDto.Type,
                PhysicalExaminations = null,
                LabExaminations = null
            };

            return new JsonResult(await examinationCodeRepository.AddAsync(examinationCode)) {StatusCode = 201};

        }
    }
}
