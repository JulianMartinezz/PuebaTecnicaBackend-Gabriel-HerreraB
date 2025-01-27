using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Validations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Impl
{
    // This class implements the interface IMedicalServices and provides methods for managing medical records.
    // It handles filtering, retrieving, creating, updating, and deleting medical records, while performing 
    // validation and mapping between entities and DTOs. It also provides appropriate response codes and messages.

    public class MedicalServicesImpl : IMedicalServices
    {
        private readonly IMedicalRepository _medicalRecordRepository;
        private readonly IMapper _mapper;
        private readonly HRDbContext _context;

        public MedicalServicesImpl(IMedicalRepository medicalRecordRepository, IMapper mapper, HRDbContext context)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _mapper = mapper;
            _context = context;
        }
        // This method retrieves a list of medical records filtered by status, date range, and medical record type.
        // It supports pagination with the provided page and pageSize parameters. It returns a response with the filtered 
        // medical records and the total count, or an error message if no records are found or invalid parameters are provided.
        public async Task<BaseResponse<List<GetMedicalDto>>> GetFilterMedicalRecordsAsync(
              int page,
              int pageSize,
              int? statusId,
              DateOnly? startDate,
              DateOnly? endDate,
              int? medicalRecordTypeId)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BaseResponse<List<GetMedicalDto>>
                {
                    Success = false,
                    Message = "Invalid page or page size parameters.",
                    Code = 400
                };
            }

            var query = _context.t_medical_records
                .Include(m => m.status)
                .Include(m => m.medical_record_type)
                .Where(m =>
                    (statusId == null || m.status_id == statusId) &&
                    (startDate == null || m.start_date >= startDate) &&
                    (endDate == null || m.end_date <= endDate) &&
                    (medicalRecordTypeId == null || m.medical_record_type_id == medicalRecordTypeId)
                );

            var totalRecords = await query.CountAsync();

            var records = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (records == null || !records.Any())
            {
                return new BaseResponse<List<GetMedicalDto>>
                {
                    Success = false,
                    Message = "No medical records found.",
                    Code = 404,
                    TotalRows = 0
                };
            }

            var recordDtos = _mapper.Map<List<GetMedicalDto>>(records);

            return new BaseResponse<List<GetMedicalDto>>
            {
                Success = true,
                Message = "Medical records retrieved successfully.",
                Data = recordDtos,
                Code = 200,
                TotalRows = totalRecords
            };
        }
        // This method retrieves a medical record by its ID and maps it to a DTO. 
        // It handles invalid IDs and returns appropriate responses based on whether the record is found.
        public async Task<BaseResponse<GetMedicalDto>> GetMedicalRecordByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new BaseResponse<GetMedicalDto>
                {
                    Success = false,
                    Message = "Invalid medical record ID.",
                    Code = 400
                };
            }

            try
            {
                var medicalRecord = await _context.t_medical_records
                    .Include(m => m.status)
                    .Include(m => m.medical_record_type)
                    .FirstOrDefaultAsync(m => m.medical_record_id == id);

                if (medicalRecord == null)
                {
                    return new BaseResponse<GetMedicalDto>
                    {
                        Success = false,
                        Message = "Medical record not found.",
                        Code = 404
                    };
                }

                var medicalRecordDto = _mapper.Map<GetMedicalDto>(medicalRecord);

                return new BaseResponse<GetMedicalDto>
                {
                    Success = true,
                    Message = "Medical record retrieved successfully.",
                    Data = medicalRecordDto,
                    Code = 200
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetMedicalDto>
                {
                    Success = false,
                    Message = "Error retrieving medical record.",
                    Code = 500,
                    Exception = ex.Message
                };
            }
        }
        // This method creates a new medical record based on the provided DTO. 
        // It validates the input, adds the record to the database, and returns a response with the created record.
        public async Task<BaseResponse<GetMedicalDto>> AddMedicalRecordAsync(CreateMedicalDto createDto)
        {
            var validator = new CreateMedicalDtoValidator(_context);
            var validationResult = await validator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return new BaseResponse<GetMedicalDto>
                {
                    Success = false,
                    Message = "Validation failed",
                    Code = 400,
                    Exception = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            createDto.StatusId = 1;

            var entity = _mapper.Map<t_medical_record>(createDto);
            entity.creation_date = DateOnly.FromDateTime(DateTime.UtcNow);
            entity.created_by = createDto.CreatedBy;
            await _medicalRecordRepository.AddMedicalRecordAsync(entity);

            var resultDto = _mapper.Map<GetMedicalDto>(entity);
            return new BaseResponse<GetMedicalDto>
            {
                Success = true,
                Message = "Medical record created successfully",
                Data = resultDto,
                Code = 200
            };
        }
        // This method updates an existing medical record based on the provided DTO. 
        // It validates the input, maps the changes, and saves the updated record to the database.
        public async Task<BaseResponse<GetMedicalDto>> UpdateMedicalRecordAsync(UpdateMedicalDto updateDto)
        {
            var validator = new UpdateMedicalDtoValidator(_context);
            var validationResult = await validator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return new BaseResponse<GetMedicalDto>
                {
                    Success = false,
                    Message = "Validation failed",
                    Code = 400,
                    Exception = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            var existingRecord = await _medicalRecordRepository.GetMedicalRecordByIdAsync(updateDto.Id);
            if (existingRecord == null)
            {
                return new BaseResponse<GetMedicalDto>
                {
                    Success = false,
                    Message = "Medical record not found",
                    Code = 404
                };
            }

            _mapper.Map(updateDto, existingRecord);
            existingRecord.modification_date = DateOnly.FromDateTime(DateTime.UtcNow);
            existingRecord.modified_by = updateDto.ModifiedBy;

            await _medicalRecordRepository.UpdateMedicalRecordAsync(existingRecord);

            var resultDto = _mapper.Map<GetMedicalDto>(existingRecord);
            return new BaseResponse<GetMedicalDto>
            {
                Success = true,
                Message = "Medical record updated successfully.",
                Data = resultDto,
                Code = 200
            };
        }
        // This method deletes a medical record based on the provided deletion DTO. 
        // It validates the input, soft deletes the record, and returns a success or failure response.
        public async Task<BaseResponse<bool>> DeleteMedicalRecordAsync(DeleteMedicalDto deleteDto)
        {
            var validator = new DeleteMedicalDtoValidator();
            var validationResult = await validator.ValidateAsync(deleteDto);

            if (!validationResult.IsValid)
            {
                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = false,
                    Code = 400
                };
            }
            
            bool deleted = await _medicalRecordRepository.DeleteMedicalRecordAsync( deleteDto);

            if (!deleted)
            {
                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "Failed to delete medical record or record not found",
                    Data = false,
                    Code = 404
                };
            }

            return new BaseResponse<bool>
            {
                Success = true,
                Message = "Medical record deleted successfully",
                Data = true,
                Code = 200
            };
        }
    }
}
