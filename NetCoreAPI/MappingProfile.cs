using AutoMapper;
using NetCoreAPI.Models;
using NetCoreAPI.DTOs;

namespace NetCoreAPI.Mapping;

/// <summary>
/// Profil AutoMapper définissant les correspondances entre les modèles du domaine et les DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Configure toutes les correspondances bidirectionnelles entre modèles et DTOs.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserCreationUpdateDto, User>().ReverseMap();

        CreateMap<Subscription, SubscriptionDto>().ReverseMap();
        CreateMap<Session, SessionDto>().ReverseMap();
        CreateMap<Result, ResultDto>().ReverseMap();
        CreateMap<Module, ModuleDto>().ReverseMap();
        CreateMap<Formation, FormationDto>().ReverseMap();
        CreateMap<Evaluation, EvaluationDto>().ReverseMap();
        CreateMap<Airecommandation, AirecommandationDto>().ReverseMap();
    }
}
