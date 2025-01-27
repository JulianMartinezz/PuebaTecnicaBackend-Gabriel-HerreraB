
using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IMedicalServices
    {
        // This interface defines the contract for medical services operations, including methods for filtering, 
        // retrieving, creating, updating, and deleting medical records. Each method returns a standardized response 
        // containing relevant data or an error message, ensuring consistency in communication across services.
        Task<BaseResponse<List<GetMedicalDto>>> GetFilterMedicalRecordsAsync(int page, int pageSize, int? statusId, 
            DateOnly? startDate, DateOnly? endDate, int? medicalRecordTypeId);
        Task<BaseResponse<GetMedicalDto>> GetMedicalRecordByIdAsync(int id);
        Task<BaseResponse<GetMedicalDto>> AddMedicalRecordAsync(CreateMedicalDto createDto);
        Task<BaseResponse<GetMedicalDto>> UpdateMedicalRecordAsync(UpdateMedicalDto updateDto);
        Task<BaseResponse<bool>> DeleteMedicalRecordAsync(DeleteMedicalDto deleteMedicalDto);
    }
}
