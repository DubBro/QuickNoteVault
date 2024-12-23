﻿using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Newtonsoft.Json.Linq;
using QuickNoteVault.BLL;
using QuickNoteVault.BLL.Models;
using QuickNoteVault.BLL.Services;
using QuickNoteVault.BLL.Services.Interfaces;
using QuickNoteVault.DAL.Entities;
using QuickNoteVault.DAL.Repository;
using QuickNoteVault.DAL.UOW;
using QuickNoteVault.Infrastructure.Exceptions;

namespace QuickNoteVault.UnitTests.Services;

public class NoteServiceTests
{
    private readonly INoteService _noteService;

    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IRepository<NoteEntity>> _notes;
    private readonly Mock<IDbContextTransaction> _dbTransaction;

    private readonly IMapper _mapper;

    private readonly ICollection<NoteEntity> _fakeNoteEntities = new List<NoteEntity>
    {
        new NoteEntity
        {
            Id = 1,
            Title = "Note 1",
            Content = "[]",
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 1
        },
        new NoteEntity
        {
            Id = 2,
            Title = "Note 2",
            Content = "[]",
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 1
        },
        new NoteEntity
        {
            Id = 3,
            Title = "Note 3",
            Content = "[]",
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 2
        }
    };

    private readonly ICollection<NoteModel> _fakeNoteModels = new List<NoteModel>
    {
        new NoteModel
        {
            Id = 1,
            Title = "Note 1",
            Content = new JArray(),
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 1
        },
        new NoteModel
        {
            Id = 2,
            Title = "Note 2",
            Content = new JArray(),
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 1
        },
        new NoteModel
        {
            Id = 3,
            Title = "Note 3",
            Content = new JArray(),
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
            UserId = 2
        }
    };

    private readonly NoteEntity _fakeNoteEntity = new NoteEntity()
    {
        Id = 1,
        Title = "Fake Note",
        Content = "[]",
        CreatedAt = DateTime.MinValue,
        ModifiedAt = DateTime.MinValue,
        UserId = 1
    };

    private NoteModel _fakeNoteModel = new NoteModel()
    {
        Id = 1,
        Title = "Fake Note",
        Content = new JArray(),
        CreatedAt = DateTime.MinValue,
        ModifiedAt = DateTime.MinValue,
        UserId = 1,
    };

    public NoteServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _notes = new Mock<IRepository<NoteEntity>>();
        _dbTransaction = new Mock<IDbContextTransaction>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddBLLMaps());
        _mapper = mapperConfig.CreateMapper();

        _unitOfWork.Setup(uow => uow.GetRepository<NoteEntity>())
            .Returns(_notes.Object);

        _unitOfWork.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_dbTransaction.Object);

        _noteService = new NoteService(_unitOfWork.Object, _mapper);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public async Task GetAllByUserIdAsync_PassingValidUserId_ReturnsNoteModelList(int userId)
    {
        // Arrange
        _notes.Setup(n => n.GetAllAsync(e => e.UserId == userId)).ReturnsAsync(_fakeNoteEntities);

        // Act
        var noteModels = await _noteService.GetAllByUserIdAsync(userId);

        // Assert
        noteModels.Should().BeEquivalentTo(_fakeNoteModels);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetAllByUserIdAsync_PassingValidUserId_ReturnsNoteModelListWithCorrectCountOfNoteModels(int userId)
    {
        // Arrange
        var expectedNotes = _fakeNoteEntities.Where(n => n.UserId == userId).ToList();

        _notes.Setup(n => n.GetAllAsync(e => e.UserId == userId)).ReturnsAsync(expectedNotes);

        // Act
        var noteModels = await _noteService.GetAllByUserIdAsync(userId);

        // Assert
        Assert.Equal(expectedNotes.Count, noteModels.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAllByUserIdAsync_PassingInvalidUserId_ReturnsEmptyNoteModelList(int userId)
    {
        // Arrange
        var emptyNoteEntityList = new List<NoteEntity>();

        _notes.Setup(n => n.GetAllAsync(e => e.UserId == userId)).ReturnsAsync(emptyNoteEntityList);

        // Act
        var noteModels = await _noteService.GetAllByUserIdAsync(userId);

        // Assert
        noteModels.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public async Task GetByIdAsync_PassingValidId_ReturnsNoteModel(int id)
    {
        // Arrange
        _notes.Setup(n => n.GetByIdAsync(id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var noteModel = await _noteService.GetByIdAsync(id);

        // Assert
        noteModel.Should().BeEquivalentTo(_fakeNoteModel);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_PassingInvalidId_ThrowsNoteNotFoundException(int id)
    {
        // Arrange
        _notes.Setup(n => n.GetByIdAsync(id)).ReturnsAsync((NoteEntity)null!);

        // Act
        var action = async () => await _noteService.GetByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Fact]
    public async Task AddAsync_WhenNoteModelIsNull_ThrowsNoteNotFoundException()
    {
        // Arrange
        _fakeNoteModel = null!;

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Fact]
    public async Task AddAsync_WhenNoteModelContentIsNull_ThrowsNoteNotFoundException()
    {
        // Arrange
        _fakeNoteModel.Content = null!;

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AddAsync_PassingInvalidNoteModelUserId_ThrowsUserNotFoundException(int userId)
    {
        // Arrange
        _fakeNoteModel.UserId = userId;

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(action);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_ReturnsAddedNoteModelId()
    {
        // Arrange
        _notes.Setup(n => n.AddAsync(It.IsAny<NoteEntity>())).ReturnsAsync(_fakeNoteEntity);

        // Act
        var addedNoteId = await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        addedNoteId.Should().Be(_fakeNoteEntity.Id);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.AddAsync(It.IsAny<NoteEntity>())).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsNoteRepositoryAddAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.AddAsync(It.IsAny<NoteEntity>())).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _notes.Verify(n => n.AddAsync(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsUnitOfWorkSaveAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.AddAsync(It.IsAny<NoteEntity>())).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsDbContextTransactionCommitAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.AddAsync(It.IsAny<NoteEntity>())).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _dbTransaction.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenDbContextSaveChangesAsyncError_ThrowsDbUpdateException()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
    }

    [Fact]
    public async Task AddAsync_WhenDbContextSaveChangesAsyncError_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenDbContextSaveChangesAsyncError_CallsNoteRepositoryAddAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _notes.Verify(n => n.AddAsync(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenDbContextSaveChangesAsyncError_CallsDbContextTransactionRollbackAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);

        // Act
        var action = async () => await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _dbTransaction.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNoteModelIsNull_ThrowsNoteNotFoundException()
    {
        // Arrange
        _fakeNoteModel = null!;

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Fact]
    public async Task UpdateAsync_WhenNoteModelContentIsNull_ThrowsNoteNotFoundException()
    {
        // Arrange
        _fakeNoteModel.Content = null!;

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(1111111)]
    public async Task UpdateAsync_PassingNoteModelWithInvalidOrNonExistentId_ThrowsNoteNotFoundException(int id)
    {
        // Arrange
        _fakeNoteModel.Id = id;

        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync((NoteEntity)null!);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(1111111)]
    public async Task UpdateAsync_PassingNoteModelWithInvalidOrNonExistentId_CallsNoteRepositoryFirstOrDefaultAsNoTrackingAsyncOnce(int id)
    {
        // Arrange
        _fakeNoteModel.Id = id;

        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync((NoteEntity)null!);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
        _notes.Verify(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(1111111)]
    public async Task UpdateAsync_PassingNoteModelWithInvalidOrNonExistentUserId_ThrowsUserNotFoundException(int userId)
    {
        // Arrange
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        _fakeNoteModel.UserId = userId;

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(1111111)]
    public async Task UpdateAsync_PassingNoteModelWithInvalidOrNonExistentUserId_CallsNoteRepositoryFirstOrDefaultAsNoTrackingAsyncOnce(int userId)
    {
        // Arrange
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        _fakeNoteModel.UserId = userId;

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(action);
        _notes.Verify(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PassingValidNoteModel_ReturnsUpdatedNoteModelId()
    {
        // Arrange
        _notes.Setup(n => n.Update(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var updatedNoteId = await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        updatedNoteId.Should().Be(_fakeNoteEntity.Id);
    }

    [Fact]
    public async Task UpdateAsync_PassingValidNoteModel_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Update(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PassingValidNoteModel_CallsNoteRepositoryUpdateOnce()
    {
        // Arrange
        _notes.Setup(n => n.Update(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        _notes.Verify(n => n.Update(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PassingValidNoteModel_CallsUnitOfWorkSaveAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Update(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_PassingValidNoteModel_CallsDbContextTransactionCommitAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Update(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        _dbTransaction.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenDbContextSaveChangesAsyncError_ThrowsDbUpdateException()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
    }

    [Fact]
    public async Task UpdateAsync_WhenDbContextSaveChangesAsyncError_CallsNoteRepositoryFirstOrDefaultAsNoTrackingAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _notes.Verify(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenDbContextSaveChangesAsyncError_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenDbContextSaveChangesAsyncError_CallsNoteRepositoryUpdateOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _notes.Verify(n => n.Update(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenDbContextSaveChangesAsyncError_CallsDbContextTransactionRollbackAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.FirstOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.UpdateAsync(_fakeNoteModel);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _dbTransaction.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(111111)]
    public async Task DeleteByIdAsync_PassingInvalidOrNonExistentNoteModelId_ThrowsNoteNotFoundException(int id)
    {
        // Arrange
        _notes.Setup(n => n.GetByIdAsync(id)).ReturnsAsync((NoteEntity)null!);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    [InlineData(111111)]
    public async Task DeleteByIdAsync_PassingInvalidOrNonExistentNoteModelId_CallsNoteRepositoryGetByIdAsyncOnce(int id)
    {
        // Arrange
        _notes.Setup(n => n.GetByIdAsync(id)).ReturnsAsync((NoteEntity)null!);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(action);
        _notes.Verify(n => n.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_ReturnsDeletedNoteModelId()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var deletedNoteId = await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        deletedNoteId.Should().Be(_fakeNoteEntity.Id);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_CallsNoteRepositoryGetByIdAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        _notes.Verify(n => n.GetByIdAsync(_fakeNoteModel.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_CallsNoteRepositoryDeleteOnce()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        _notes.Verify(n => n.Delete(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_CallsUnitOfWorkSaveAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        _unitOfWork.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_PassingValidNoteModelId_CallsDbContextTransactionCommitAsyncOnce()
    {
        // Arrange
        _notes.Setup(n => n.Delete(It.IsAny<NoteEntity>())).Returns(_fakeNoteEntity);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        _dbTransaction.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenDbContextSaveChangesAsyncError_ThrowsDbUpdateException()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenDbContextSaveChangesAsyncError_CallsNoteRepositoryGetByIdAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _notes.Verify(n => n.GetByIdAsync(_fakeNoteModel.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenDbContextSaveChangesAsyncError_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenDbContextSaveChangesAsyncError_CallsNoteRepositoryDeleteOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _notes.Verify(n => n.Delete(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenDbContextSaveChangesAsyncError_CallsDbContextTransactionRollbackAsyncOnce()
    {
        // Arrange
        var dbUpdateException = new DbUpdateException();

        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(dbUpdateException);
        _notes.Setup(n => n.GetByIdAsync(_fakeNoteModel.Id)).ReturnsAsync(_fakeNoteEntity);

        // Act
        var action = async () => await _noteService.DeleteByIdAsync(_fakeNoteModel.Id);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(action);
        _dbTransaction.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
