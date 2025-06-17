using API.Data_Layer.DTOs;
using API.Data_Layer.Models;
using API.Extensions;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
                .ForMember(d => d.PhotoUrl,
                o => o.MapFrom(s => s.Photos.FirstOrDefault(X => X.IsMainPhoto)!.Url));

            //Need to add this because when Photo and PhotoDTO have a prop name mismatch 
            //and we need to explicitly map that prop for it to not default to "false"
            CreateMap<Photo, PhotoDTO>()
                .ForMember(d => d.IsMain, o => o.MapFrom(s => s.IsMainPhoto));

            CreateMap<MemberUpdateDTO, AppUser>();

            CreateMap<RegisterDTO, AppUser>();
            CreateMap<string, DateOnly>()
                .ConvertUsing(s => DateOnly.Parse(s));
            CreateMap<Message, MessageDTO>()
                .ForMember(d => d.SenderPhotoUrl,
                           o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMainPhoto)!.Url))
                .ForMember(d => d.RecipientPhotoUrl,
                           o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMainPhoto)!.Url));
        }
    }
}
