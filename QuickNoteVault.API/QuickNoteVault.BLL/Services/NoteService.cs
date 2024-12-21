using AutoMapper;
using QuickNoteVault.BLL.Models;
using QuickNoteVault.BLL.Services.Interfaces;
using QuickNoteVault.DAL.Entities;
using QuickNoteVault.DAL.Repository;
using QuickNoteVault.DAL.UOW;
using QuickNoteVault.Infrastructure.Exceptions;

namespace QuickNoteVault.BLL.Services;

public class NoteService : INoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<NoteEntity> _notes;
    private readonly IMapper _mapper;

    public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _notes = _unitOfWork.GetRepository<NoteEntity>();
        _mapper = mapper;
    }

    public async Task<ICollection<NoteModel>> GetAllByUserIdAsync(int userId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<NoteModel> GetByIdAsync(int id)
    {
        var noteEntity = await _notes.GetByIdAsync(id)
            ?? throw new NoteNotFoundException();
        return _mapper.Map<NoteModel>(noteEntity);
    }

    public async Task AddAsync(NoteModel noteModel)
    {
        if (noteModel == null || noteModel.Content == null)
        {
            throw new NoteNotFoundException();
        }

        if (noteModel.UserId <= 0)
        {
            throw new UserNotFoundException();
        }

        noteModel.Id = 0;
        noteModel.Title = noteModel.Title.Trim();
        noteModel.CreatedAt = DateTime.UtcNow;
        noteModel.ModifiedAt = DateTime.UtcNow;

        var noteEntity = _mapper.Map<NoteEntity>(noteModel);

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _notes.AddAsync(noteEntity);

            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(NoteModel noteModel)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(int id)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
