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
                .ForMember(dest => dest.ExerciseTemplateId, cfg => cfg.MapFrom(src => src.Template.ExerciseTemplateId))
                .ForMember(dest => dest.CategoryIds, cfg => cfg.MapFrom(src => src.ExerciseExerciseCategories
                                                                                  .Where(x => x.ExerciseId == src.ExerciseId)
                                                                                  .Select(x => x.ExerciseCategoryId)))
                .ReverseMap();

            CreateMap<ExerciseCategory, ExerciseCategoryViewModel>()
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.ExerciseCategoryId))
                .ForMember(dest => dest.ExerciseIds, cfg => cfg.MapFrom(src => src.ExerciseExerciseCategories
                                                                                  .Where(x => x.ExerciseCategoryId == src.ExerciseCategoryId)
                                                                                  .Select(x => x.ExerciseId)))
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
                                                                                                                    }))).ReverseMap();

            // Database View Models to Database Models
            CreateMap<TestViewModel, Test>()
                .ForMember(dest => dest.TestId, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(dest => dest.TestOutput, cfg => cfg.MapFrom(src => new TestOutput()
                                                                                  {
                                                                                      TestOutputId = src.TestOutput.Id,
                                                                                      TestForeignKey = src.Id.GetValueOrDefault(),
                                                                                      Value = src.TestOutput.Value,
                                                                                      ValueType = src.TestOutput.ValueType
                                                                                  }))
                .ForMember(dest => dest.TestInputs, cfg => cfg.MapFrom(src => src.TestInputs.Select(testInput => new TestInput()
                                                                                                                     {
                                                                                                                         TestInputId = testInput.Id.GetValueOrDefault(),
                                                                                                                         ArgumentName = testInput.ArgumentName,
                                                                                                                         Value = testInput.Value,
                                                                                                                         ValueType = testInput.ValueType
                                                                                                                     })));
        }
    }
}

