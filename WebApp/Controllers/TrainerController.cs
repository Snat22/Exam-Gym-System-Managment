using Domain.DTOs.TrainerDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.TrainerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainerController(ITrainerService TrainerService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<Response<List<GetTrainerDto>>> GetTrainersAsync([FromQuery] TrainerFilter TrainerFilter)
        => await TrainerService.GetTrainersAsync(TrainerFilter);

    [HttpGet("{TrainerId:int}")]
    [AllowAnonymous]
    public async Task<Response<GetTrainerDto>> GetTrainerByIdAsync(int TrainerId)
        => await TrainerService.GetTrainerByIdAsync(TrainerId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateTrainerAsync([FromForm] CreateTrainerDto Trainer)
        => await TrainerService.CreateTrainerAsync(Trainer);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateTrainerAsync([FromForm] UpdateTrainerDto Trainer)
        => await TrainerService.UpdateTrainerAsync(Trainer);

    [HttpDelete("{TrainerId:int}")]
    public async Task<Response<bool>> DeleteTrainerAsync(int TrainerId)
        => await TrainerService.DeleteTrainerAsync(TrainerId);
}