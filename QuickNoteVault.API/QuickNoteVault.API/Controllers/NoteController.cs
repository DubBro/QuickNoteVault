﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuickNoteVault.API.DTOs.Note;
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

    [HttpGet("{id:int}")]
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

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetAll(int userId)
    {
        try
        {
            var noteModelList = await _noteService.GetAllByUserIdAsync(userId);
            return Ok(_mapper.Map<ICollection<NoteListItemDTO>>(noteModelList));
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(NoteAddItemDTO noteDTO)
    {
        try
        {
            var noteModel = _mapper.Map<NoteModel>(noteDTO);
            var addedNoteModelId = await _noteService.AddAsync(noteModel);
            return Ok(addedNoteModelId);
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
    public async Task<IActionResult> Update(NoteUpdateItemDTO noteDTO)
    {
        try
        {
            var noteModel = _mapper.Map<NoteModel>(noteDTO);
            var updatedNoteModelId = await _noteService.UpdateAsync(noteModel);
            return Ok(updatedNoteModelId);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedNoteModelId = await _noteService.DeleteByIdAsync(id);
            return Ok(deletedNoteModelId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"NoteController - {ex.Message}");

            if (ex is NoteNotFoundException)
            {
                return BadRequest();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}
