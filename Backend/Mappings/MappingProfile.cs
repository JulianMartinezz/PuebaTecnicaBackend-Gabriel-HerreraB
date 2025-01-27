using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<t_medical_record, BaseMedicalRecordDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.start_date))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.end_date))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.status_id))
                .ForMember(dest => dest.MedicalRecordTypeId, opt => opt.MapFrom(src => src.medical_record_type_id))
                .ForMember(dest => dest.MedicalRecordTypeName, opt => opt.MapFrom(src => src.medical_record_type.name))
                .ForMember(dest => dest.AreaChange, opt => opt.MapFrom(src => src.area_change))
                .ForMember(dest => dest.VoiceEvaluation, opt => opt.MapFrom(src => src.voice_evaluation))
                .ForMember(dest => dest.ExecuteExtra, opt => opt.MapFrom(src => src.execute_extra))
                .ForMember(dest => dest.ExecuteMicros, opt => opt.MapFrom(src => src.execute_micros))
                .ForMember(dest => dest.Audiometry, opt => opt.MapFrom(src => src.audiometry))
                .ForMember(dest => dest.MedicalBoard, opt => opt.MapFrom(src => src.medical_board))
                .ForMember(dest => dest.OtherFamilyData, opt => opt.MapFrom(src => src.other_family_data))
                .ForMember(dest => dest.FatherData, opt => opt.MapFrom(src => src.father_data))
                .ForMember(dest => dest.MotherData, opt => opt.MapFrom(src => src.mother_data))
                .ForMember(dest => dest.PositionChange, opt => opt.MapFrom(src => src.position_change))
                .ForMember(dest => dest.Disability, opt => opt.MapFrom(src => src.disability))
                .ForMember(dest => dest.DisabilityPercentage, opt => opt.MapFrom(src => src.disability_percentage))
                .ForMember(dest => dest.Observations, opt => opt.MapFrom(src => src.observations))
                .ReverseMap();

            CreateMap<t_medical_record, CreateMedicalDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.created_by))
                .IncludeBase<t_medical_record, BaseMedicalRecordDto>()
                .ReverseMap();

            CreateMap<t_medical_record, UpdateMedicalDto>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.modified_by))
                .IncludeBase<t_medical_record, BaseMedicalRecordDto>()
                .ReverseMap();


            CreateMap<t_medical_record, GetMedicalDto>()
               .ForMember(dest => dest.MedicalRecordId, opt => opt.MapFrom(src => src.medical_record_id))
               .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.created_by))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.modified_by))
               .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.deleted_by))
               .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.creation_date))
               .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => src.modification_date))
               .ForMember(dest => dest.DeletionDate, opt => opt.MapFrom(src => src.deletion_date))
               .IncludeBase<t_medical_record, BaseMedicalRecordDto>();
        }
    }
}
