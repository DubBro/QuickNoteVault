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
        var noteEntityList = await _notes.GetAllAsync(n => n.UserId == userId);
        return _mapper.Map<ICollection<NoteModel>>(noteEntityList);
    }

    public async Task<NoteModel> GetByIdAsync(int id)
    {
        var noteEntity = await _notes.GetByIdAsync(id)
            ?? throw new NoteNotFoundException();
        return _mapper.Map<NoteModel>(noteEntity);
    }

    public async Task<int> AddAsync(NoteModel noteModel)
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
            var addedNoteEntity = await _notes.AddAsync(noteEntity);

            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return addedNoteEntity.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> UpdateAsync(NoteModel noteModel)
    {
        if (noteModel == null || noteModel.Content == null)
        {
            throw new NoteNotFoundException();
        }

        var oldNoteEntity = await _notes.FirstOrDefaultAsNoTrackingAsync(e => e.Id == noteModel.Id)
            ?? throw new NoteNotFoundException();

        if (oldNoteEntity.UserId != noteModel.UserId)
        {
            throw new UserNotFoundException();
        }

        var newNoteEntity = _mapper.Map<NoteEntity>(noteModel);

        newNoteEntity.Title = newNoteEntity.Title.Trim();
        newNoteEntity.CreatedAt = oldNoteEntity.CreatedAt;
        newNoteEntity.ModifiedAt = DateTime.UtcNow;

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var updatedNoteEntity = _notes.Update(newNoteEntity);

            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return updatedNoteEntity.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> DeleteByIdAsync(int id)
    {
        var noteEntity = await _notes.GetByIdAsync(id)
            ?? throw new NoteNotFoundException();

        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var deletedNoteEntity = _notes.Delete(noteEntity);

            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return deletedNoteEntity.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
