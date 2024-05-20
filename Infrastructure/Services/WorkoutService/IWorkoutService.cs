using Domain.DTOs.WorkoutDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.WorkoutService;

public interface IWorkoutService
{
    Task<PagedResponse<List<GetWorkoutDto>>> GetWorkoutsAsync(WorkoutFilter filter);
    Task<Response<GetWorkoutDto>> GetWorkoutByIdAsync(int WorkoutId);
    Task<Response<string>> CreateWorkoutAsync(CreateWorkoutDto createWorkout);
    Task<Response<string>> UpdateWorkoutAsync(UpdateWorkoutDto updateWorkout);
    Task<Response<bool>> DeleteWorkoutAsync(int WorkoutId);
}
