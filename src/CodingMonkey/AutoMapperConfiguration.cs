namespace CodingMonkey
{
    using AutoMapper;
    using CodingMonkey.Models;
    using CodingMonkey.ViewModels;

    public class AutoMapperConfiguration : Profile
    {
        protected override void Configure()
        {
            // Database Models to Database View Models
            CreateMap<Exercise, ExerciseViewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ExerciseId));

            CreateMap<ExerciseCategory, ExerciseCategoryViewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ExerciseCategoryId));

            CreateMap<ExerciseTemplate, ExerciseTemplateViewModel>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ExerciseTemplateId))
                .ForMember(dest => dest.ExerciseId, opts => opts.MapFrom(src => src.ExerciseForeignKey));
        }
    }
}
