using Domain.DTOs.WorkoutDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.WorkoutService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutController(IWorkoutService WorkoutService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<Response<List<GetWorkoutDto>>> GetWorkoutsAsync([FromQuery] WorkoutFilter WorkoutFilter)
        => await WorkoutService.GetWorkoutsAsync(WorkoutFilter);

    [HttpGet("{WorkoutId:int}")]
    [AllowAnonymous]
    public async Task<Response<GetWorkoutDto>> GetWorkoutByIdAsync(int WorkoutId)
        => await WorkoutService.GetWorkoutByIdAsync(WorkoutId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateWorkoutAsync([FromForm] CreateWorkoutDto Workout)
        => await WorkoutService.CreateWorkoutAsync(Workout);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateWorkoutAsync([FromForm] UpdateWorkoutDto Workout)
        => await WorkoutService.UpdateWorkoutAsync(Workout);

    [HttpDelete("{WorkoutId:int}")]
    public async Task<Response<bool>> DeleteWorkoutAsync(int WorkoutId)
        => await WorkoutService.DeleteWorkoutAsync(WorkoutId);
}
