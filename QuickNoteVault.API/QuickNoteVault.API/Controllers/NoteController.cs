using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuickNoteVault.API.DTOs;
using QuickNoteVault.BLL.Models;
using QuickNoteVault.BLL.Services.Interfaces;
using QuickNoteVault.Infrastructure.Exceptions;

namespace QuickNoteVault.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NoteController> _logger;
    private readonly IMapper _mapper;

    public NoteController(
        INoteService noteService,
        ILogger<NoteController> logger,
        IMapper mapper)
    {
        _noteService = noteService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var noteModel = await _noteService.GetByIdAsync(id);
            return Ok(_mapper.Map<NoteDTO>(noteModel));
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException)
            {
                return NotFound();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int userId)
    {
        try
        {
            var noteModelList = await _noteService.GetAllByUserIdAsync(userId);
            return Ok(_mapper.Map<List<NoteDTO>>(noteModelList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException)
            {
                return NotFound();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(NoteDTO noteDTO)
    {
        try
        {
            var noteModel = _mapper.Map<NoteModel>(noteDTO);
            await _noteService.AddAsync(noteModel);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException || ex is UserNotFoundException)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(NoteDTO noteDTO)
    {
        try
        {
            var noteModel = _mapper.Map<NoteModel>(noteDTO);
            await _noteService.UpdateAsync(noteModel);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException || ex is UserNotFoundException)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _noteService.DeleteByIdAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException)
            {
                return NotFound();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
