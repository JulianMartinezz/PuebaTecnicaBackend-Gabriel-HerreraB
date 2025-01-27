using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// This interface defines methods for interacting with medical records in the repository.
// It includes methods for retrieving, adding, updating, and soft deleting medical records, 
// as well as filtering records based on various criteria.
public interface IMedicalRepository
{
    Task<List<t_medical_record>> GetFilterMedicalRecordsAsync(int page, int pageSize, int? statusId,
        DateOnly? startDate, DateOnly? endDate, int? medicalTypeId);
    Task<t_medical_record> GetMedicalRecordByIdAsync(int id);
    Task AddMedicalRecordAsync(t_medical_record medical);
    Task UpdateMedicalRecordAsync(t_medical_record medical);
    Task<bool> DeleteMedicalRecordAsync( DeleteMedicalDto delete);
}
