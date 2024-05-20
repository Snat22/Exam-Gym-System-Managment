using System.Net;
using Domain.DTOs.WorkoutDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.WorkoutService;

public class WorkoutService(DataContext context) : IWorkoutService
{
    #region GetWorkoutsAsync

    public async Task<PagedResponse<List<GetWorkoutDto>>> GetWorkoutsAsync(WorkoutFilter filter)
    {
        try
        {
            var Workouts = context.Workouts.Include(x => x.ClassSchedules).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                Workouts = Workouts.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));


            var response = await Workouts.Select(x => new GetWorkoutDto()
            {
                Name = x.Name,
                Description = x.Description,
                Intensity = x.Intensity,
                Duration = x.Duration,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await Workouts.CountAsync();

            return new PagedResponse<List<GetWorkoutDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetWorkoutDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetWorkoutByIdAsync

    public async Task<Response<GetWorkoutDto>> GetWorkoutByIdAsync(int WorkoutId)
    {
        try
        {
            var existing = await context.Workouts.FirstOrDefaultAsync(x => x.Id == WorkoutId);
            if (existing == null) return new Response<GetWorkoutDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetWorkoutDto()
            {
                Name = existing.Name,
                Description = existing.Description,
                Intensity = existing.Intensity,
                Duration = existing.Duration,
                Id = existing.Id,
            };
            return new Response<GetWorkoutDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetWorkoutDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateWorkoutAsync

    public async Task<Response<string>> CreateWorkoutAsync(CreateWorkoutDto createWorkout)
    {
        try
        {
            var Workout = new Workout()
            {
                Name = createWorkout.Name,
                Description = createWorkout.Description,
                Intensity = createWorkout.Intensity,
                Duration = createWorkout.Duration,
            };
            await context.Workouts.AddAsync(Workout);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created Workout");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateWorkoutAsync

    public async Task<Response<string>> UpdateWorkoutAsync(UpdateWorkoutDto updateWorkout)
    {
        try
        {
            var existingWorkout = await context.Workouts.FirstOrDefaultAsync(x => x.Id == updateWorkout.Id);
            if (existingWorkout == null) return new Response<string>(HttpStatusCode.BadRequest, "Workout not found");

            existingWorkout.Name = updateWorkout.Name;
            existingWorkout.Description = updateWorkout.Description;
            existingWorkout.Duration = updateWorkout.Duration;
            existingWorkout.Intensity = updateWorkout.Intensity;

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the Workout");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteWorkoutAsync

    public async Task<Response<bool>> DeleteWorkoutAsync(int WorkoutId)
    {
        try
        {
            var existing = await context.Workouts.FirstOrDefaultAsync(x => x.Id == WorkoutId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            context.Workouts.Remove(existing);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}