using Domain.DTOs.TrainerDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.TrainerService;

public interface ITrainerService
{
    Task<PagedResponse<List<GetTrainerDto>>> GetTrainersAsync(TrainerFilter filter);
    Task<Response<GetTrainerDto>> GetTrainerByIdAsync(int TrainerId);
    Task<Response<string>> CreateTrainerAsync(CreateTrainerDto createTrainer);
    Task<Response<string>> UpdateTrainerAsync(UpdateTrainerDto updateTrainer);
    Task<Response<bool>> DeleteTrainerAsync(int TrainerId);
}
