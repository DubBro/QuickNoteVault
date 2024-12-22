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
    public async Task AddAsync_PassingValidNoteModel_CallsUnitOfWorkBeginTransactionAsyncOnce()
    {
        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsNoteRepositoryAddAsyncOnce()
    {
        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _notes.Verify(n => n.AddAsync(It.IsAny<NoteEntity>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsUnitOfWorkSaveAsyncOnce()
    {
        // Act
        await _noteService.AddAsync(_fakeNoteModel);

        // Assert
        _unitOfWork.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_PassingValidNoteModel_CallsDbContextTransactionCommitAsyncOnce()
    {
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
}
