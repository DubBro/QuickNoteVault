using AutoMapper;
using QuickNoteVault.API.DTOs.Note;
using QuickNoteVault.BLL.Models;

namespace QuickNoteVault.API.Utils.Automapper;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteModel, NoteDTO>();

        CreateMap<NoteModel, NoteListItemDTO>();

        CreateMap<NoteAddItemDTO, NoteModel>();

        CreateMap<NoteUpdateItemDTO, NoteModel>();
    }
}
