using System.Net;
using Domain.DTOs.TrainerDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TrainerService;

public class TrainerService(DataContext context, IFileService fileService) : ITrainerService
{

    public async Task<PagedResponse<List<GetTrainerDto>>> GetTrainersAsync(TrainerFilter filter)
    {
        try
        {
            var Trainers = context.Trainers.Include(x => x.ClassSchedules).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                Trainers = Trainers.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));


            var response = await Trainers.Select(x => new GetTrainerDto()
            {
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Specialization = x.Specialization,
                Photo = x.Photo,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalRecord = await Trainers.CountAsync();

            return new PagedResponse<List<GetTrainerDto>>(response, totalRecord, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetTrainerDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<GetTrainerDto>> GetTrainerByIdAsync(int TrainerId)
    {
        try
        {
            var existing = await context.Trainers.FirstOrDefaultAsync(x => x.Id == TrainerId);
            if (existing == null) return new Response<GetTrainerDto>(HttpStatusCode.BadRequest, "Not Found");
            var response = new GetTrainerDto()
            {
                Name = existing.Name,
                Email = existing.Email,
                PhoneNumber = existing.PhoneNumber,
                Specialization = existing.Specialization,
                Photo = existing.Photo,
                Id = existing.Id,
            };
            return new Response<GetTrainerDto>(response);
        }
        catch (Exception e)
        {
            return new Response<GetTrainerDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<string>> CreateTrainerAsync(CreateTrainerDto createTrainer)
    {
        try
        {
            var Trainer = new Trainer()
            {
                Name = createTrainer.Name,
                Email = createTrainer.Email,
                Specialization = createTrainer.Specialization,
                PhoneNumber = createTrainer.PhoneNumber,
                Photo = await fileService.CreateFile(createTrainer.Photo),
            };
            await context.Trainers.AddAsync(Trainer);
            await context.SaveChangesAsync();
            return new Response<string>("Successfully created Trainer");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<string>> UpdateTrainerAsync(UpdateTrainerDto updateTrainer)
    {
        try
        {
            var existingTrainer = await context.Trainers.FirstOrDefaultAsync(x => x.Id == updateTrainer.Id);
            if (existingTrainer == null) return new Response<string>(HttpStatusCode.BadRequest, "Trainer not found");

            if (updateTrainer.Photo != null)
            {
                if (existingTrainer.Photo != null) fileService.DeleteFile(existingTrainer.Photo);
                existingTrainer.Photo = await fileService.CreateFile(updateTrainer.Photo);
            }

            existingTrainer.Name = updateTrainer.Name;
            existingTrainer.PhoneNumber = updateTrainer.PhoneNumber;
            existingTrainer.Email = updateTrainer.Email;
            if (updateTrainer.Photo != null)
                existingTrainer.Photo = await fileService.CreateFile(updateTrainer.Photo);
            existingTrainer.Specialization = updateTrainer.Specialization;

            await context.SaveChangesAsync();
            return new Response<string>("Successfully updated the Trainer");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }



    public async Task<Response<bool>> DeleteTrainerAsync(int TrainerId)
    {
        try
        {
            var existing = await context.Trainers.FirstOrDefaultAsync(x => x.Id == TrainerId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            if (existing.Photo != null) fileService.DeleteFile(existing.Photo);
            context.Trainers.Remove(existing);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    
}