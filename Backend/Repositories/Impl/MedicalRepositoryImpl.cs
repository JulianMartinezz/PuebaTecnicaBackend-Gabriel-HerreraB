using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MedicalRepositoryImpl : IMedicalRepository
{
    private readonly HRDbContext _context;

    public MedicalRepositoryImpl(HRDbContext context)
    {
        _context = context;
    }
    // This method retrieves a filtered list of medical records
    // from the database based on optional filters such as status, start date, end date, and medical type. 
    // It supports pagination by using page and pageSize parameters.
    public async Task<List<t_medical_record>> GetFilterMedicalRecordsAsync(int page, int pageSize,
    int? statusId, DateOnly? startDate, DateOnly? endDate, int? medicalTypeId)
    {
        return await _context.t_medical_records
            .Where(m =>
                (statusId == null || m.status_id == statusId) &&
                (startDate == null || m.start_date >= startDate) &&
                (endDate == null || m.end_date <= endDate) &&
                (medicalTypeId == null || m.medical_record_type_id == medicalTypeId))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    // This method retrieves a medical record by its ID from the database.
    public async Task<t_medical_record> GetMedicalRecordByIdAsync(int id)
    {
        return await _context.t_medical_records.FindAsync(id);
    }
    // This method adds a new medical record to the database and saves the changes.
    public async Task AddMedicalRecordAsync(t_medical_record medical)
    {
        await _context.t_medical_records.AddAsync(medical);
        await _context.SaveChangesAsync();
    }
    // This method updates an existing medical record in the database and saves the changes.
    public async Task UpdateMedicalRecordAsync(t_medical_record medical)
    {
        _context.t_medical_records.Update(medical);
        await _context.SaveChangesAsync();
    }
    // This method soft deletes a medical record by updating its status and setting deletion details. 
    // It returns true if the record was successfully deleted, otherwise false.
    public async Task<bool> DeleteMedicalRecordAsync(DeleteMedicalDto deleteMedicalDto)
    {
        var record = await _context.t_medical_records.FindAsync(deleteMedicalDto.Id);
        if (record == null) return false;

        record.deleted_by = deleteMedicalDto.DeletedBy;
        record.deletion_reason = deleteMedicalDto.DeletionReason;
        record.deletion_date = DateOnly.FromDateTime(DateTime.UtcNow);
        record.status_id = 2;
       
        await _context.SaveChangesAsync();
        return true;
    }
}