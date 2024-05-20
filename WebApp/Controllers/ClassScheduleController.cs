using Domain.DTOs.ClassScheduleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.ClassScheduleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassScheduleController(IClassScheduleService ClassScheduleService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<Response<List<GetClassScheduleDto>>> GetClassSchedulesAsync([FromQuery] ClassScheduleFilter ClassScheduleFilter)
        => await ClassScheduleService.GetClassSchedulesAsync(ClassScheduleFilter);

    [HttpGet("{ClassScheduleId:int}")]
    [AllowAnonymous]
    public async Task<Response<GetClassScheduleDto>> GetClassScheduleByIdAsync(int ClassScheduleId)
        => await ClassScheduleService.GetClassScheduleByIdAsync(ClassScheduleId);

    [HttpPost("create")]
    public async Task<Response<string>> CreateClassScheduleAsync([FromForm] CreateClassScheduleDto ClassSchedule)
        => await ClassScheduleService.CreateClassScheduleAsync(ClassSchedule);


    [HttpPut("update")]
    public async Task<Response<string>> UpdateClassScheduleAsync([FromForm] UpdateClassScheduleDto ClassSchedule)
        => await ClassScheduleService.UpdateClassScheduleAsync(ClassSchedule);

    [HttpDelete("{ClassScheduleId:int}")]
    public async Task<Response<bool>> DeleteClassScheduleAsync(int ClassScheduleId)
        => await ClassScheduleService.DeleteClassScheduleAsync(ClassScheduleId);
}