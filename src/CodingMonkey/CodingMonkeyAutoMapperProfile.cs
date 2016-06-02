namespace CodingMonkey
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using CodingMonkey.Models;
    using CodingMonkey.ViewModels;

    public class CodingMonkeyAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            // Database Models to Database View Models
            CreateMap<Exercise, ExerciseViewModel>()
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.ExerciseId))
                .ReverseMap();

            CreateMap<ExerciseCategory, ExerciseCategoryViewModel>()
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.ExerciseCategoryId))
                .ReverseMap();

            CreateMap<ExerciseTemplate, ExerciseTemplateViewModel>()
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.ExerciseTemplateId))
                .ForMember(dest => dest.ExerciseId, cfg => cfg.MapFrom(src => src.ExerciseForeignKey))
                .ReverseMap();

            CreateMap<Test, TestViewModel>()
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.TestId))
                .ForMember(dest => dest.TestOutput, cfg => cfg.MapFrom(src => new TestOutputViewModel()
                                                                            {
                                                                                Value = src.TestOutput.Value,
                                                                                ValueType = src.TestOutput.ValueType,
                                                                                Id = src.TestOutput.TestOutputId
                                                                            }))
                .ForMember( dest => dest.TestInputs, cfg => cfg.MapFrom(src => src.TestInputs.Select(testInput => new TestInputViewModel()
                                                                                                                    {
                                                                                                                        ArgumentName = testInput.ArgumentName,
                                                                                                                        Id = testInput.TestInputId,
                                                                                                                        Value = testInput.Value,
                                                                                                                        ValueType = testInput.ValueType
                                                                                                                    }).ToList())).ReverseMap();
        }
    }
}

