using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickNoteVault.BLL.Models;
using QuickNoteVault.DAL.Entities;

namespace QuickNoteVault.BLL.Utils.Automapper;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<NoteEntity, NoteModel>()
            .ForMember(dest => dest.Content, options => options.MapFrom(src => JsonConvert.DeserializeObject<JArray>(src.Content)))
            .ReverseMap()
            .ForMember(dest => dest.Content, options => options.MapFrom(src => JsonConvert.SerializeObject(src.Content)))
            .ForMember(dest => dest.User, options => options.MapFrom<object>(_ => null!));
    }
}
