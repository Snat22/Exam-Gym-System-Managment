using System.Net;
using Domain.DTOs.ClassScheduleDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClassScheduleService;

public class ClassScheduleService(DataContext context) : IClassScheduleService
{
    #region GetClassSchedulesAsync

    public async Task<PagedResponse<List<GetClassScheduleDto>>> GetClassSchedulesAsync(ClassScheduleFilter filter)
    {
        try
        {
            var ClassSchedules = context.ClassSchedules.AsQueryable();

            if (filter.Duration != null)
                ClassSchedules = ClassSchedules.Where(x => x.Duration == filter.Duration);


            var response = await ClassSchedules.Select(x => new GetClassScheduleDto()
            {
                Date = x.Date,
                Location = x.Location,
                Duration = x.Duration,
                TrainerId = x.TrainerId,
                WorkoutId = x.WorkoutId,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await ClassSchedules.CountAsync();

            return new PagedResponse<List<GetClassScheduleDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetClassScheduleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetClassScheduleByIdAsync

    public async Task<Response<GetClassScheduleDto>> GetClassScheduleByIdAsync(int ClassScheduleId)
    {
        try
        {
            var existing = await context.ClassSchedules.FirstOrDefaultAsync(x => x.Id == ClassScheduleId);
            if (existing == null) return new Response<GetClassScheduleDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetClassScheduleDto()
            {
                Date = existing.Date,
                Location = existing.Location,
                Duration = existing.Duration,
                TrainerId = existing.TrainerId,
                WorkoutId = existing.WorkoutId,
                Id = existing.Id,
            };
            return new Response<GetClassScheduleDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetClassScheduleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateClassScheduleAsync

    public async Task<Response<string>> CreateClassScheduleAsync(CreateClassScheduleDto createClassSchedule)
    {
        try
        {
            var ClassSchedule = new ClassSchedule()
            {
                Date = createClassSchedule.Date,
                Location = createClassSchedule.Location,
                Duration = createClassSchedule.Duration,
                TrainerId = createClassSchedule.TrainerId,
                WorkoutId = createClassSchedule.WorkoutId,
            };
            await context.ClassSchedules.AddAsync(ClassSchedule);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created ClassSchedule");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateClassScheduleAsync

    public async Task<Response<string>> UpdateClassScheduleAsync(UpdateClassScheduleDto updateClassSchedule)
    {
        try
        {
            var existingClassSchedule = await context.ClassSchedules.FirstOrDefaultAsync(x => x.Id == updateClassSchedule.Id);
            if (existingClassSchedule == null) return new Response<string>(HttpStatusCode.BadRequest, "ClassSchedule not found");

            existingClassSchedule.Location = updateClassSchedule.Location;
            existingClassSchedule.Duration = updateClassSchedule.Duration;
            existingClassSchedule.WorkoutId = updateClassSchedule.WorkoutId;
            existingClassSchedule.TrainerId = updateClassSchedule.TrainerId;
            existingClassSchedule.Date = updateClassSchedule.Date;

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the ClassSchedule");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteClassScheduleAsync

    public async Task<Response<bool>> DeleteClassScheduleAsync(int ClassScheduleId)
    {
        try
        {
            var existing = await context.ClassSchedules.FirstOrDefaultAsync(x => x.Id == ClassScheduleId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            context.ClassSchedules.Remove(existing);
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
