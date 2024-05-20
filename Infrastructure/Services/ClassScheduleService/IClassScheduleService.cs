using Domain.DTOs.ClassScheduleDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.ClassScheduleService;

public interface IClassScheduleService
{
    Task<PagedResponse<List<GetClassScheduleDto>>> GetClassSchedulesAsync(ClassScheduleFilter filter);
    Task<Response<GetClassScheduleDto>> GetClassScheduleByIdAsync(int ClassScheduleId);
    Task<Response<string>> CreateClassScheduleAsync(CreateClassScheduleDto createClassSchedule);
    Task<Response<string>> UpdateClassScheduleAsync(UpdateClassScheduleDto updateClassSchedule);
    Task<Response<bool>> DeleteClassScheduleAsync(int ClassScheduleId);
}
