using AutoMapper;
using QuickNoteVault.API.DTOs;
using QuickNoteVault.BLL.Models;

namespace QuickNoteVault.API.Utils.Automapper;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteModel, NoteDTO>()
            .ReverseMap();
    }
}
