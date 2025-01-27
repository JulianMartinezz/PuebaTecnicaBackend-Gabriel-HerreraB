namespace Backend.Controllers;

using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalServices _medicalRecordService;

    public MedicalRecordController(IMedicalServices medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    // Retrieves a paginated list of medical records based on the provided filters.
    // Parameters:
    // - page: Current page number for pagination.
    // - pageSize: Number of records to display per page.
    // - statusId: (Optional) Filter for the medical record status.
    // - startDate: (Optional) Filter for the start of the date range.
    // - endDate: (Optional) Filter for the end of the date range.
    // - medicalRecordTypeId: (Optional) Filter for the type of medical record.
    // Returns: A paginated list of medical records matching the filters.
    [HttpGet("GetFilterMedicalRecords")]
    public async Task<ActionResult<BaseResponse<List<GetMedicalDto>>>> GetFilteredMedicalRecords(
         [FromQuery] int page,
         [FromQuery] int pageSize,
         [FromQuery] int? statusId = null,
         [FromQuery] DateOnly? startDate = null,
         [FromQuery] DateOnly? endDate = null,
         [FromQuery] int? medicalRecordTypeId = null)
    {
        var response = await _medicalRecordService.GetFilterMedicalRecordsAsync(page, pageSize, statusId, 
            startDate, endDate, medicalRecordTypeId);
        return StatusCode(response.Code ?? 200, response);
    }
    // Retrieves a specific medical record by its unique identifier.
    // Parameters:
    // - id: The unique identifier of the medical record to retrieve.
    // Returns: The requested medical record if found.
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<GetMedicalDto>>> GetMedicalRecordById(int id)
    {
        var response = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
        return StatusCode(response.Code ?? 200, response);
    }
    // Adds a new medical record to the system.
    // Parameters:
    // - createDto: An object containing the details of the medical record to add.
    // Returns: The newly created medical record.
    [HttpPost]
    public async Task<ActionResult<BaseResponse<GetMedicalDto>>> AddMedicalRecord([FromBody] CreateMedicalDto createDto)
    {
        var response = await _medicalRecordService.AddMedicalRecordAsync(createDto);
        return StatusCode(response.Code ?? 201, response);
    }
    // Updates an existing medical record with the provided information.
    // Parameters:
    // - updateDto: An object containing the updated details of the medical record.
    // Returns: The updated medical record.
    [HttpPut]
    public async Task<ActionResult<BaseResponse<GetMedicalDto>>> UpdateMedicalRecord(
        [FromBody] UpdateMedicalDto updateDto)
    {
        var response = await _medicalRecordService.UpdateMedicalRecordAsync( updateDto);
        return StatusCode(response.Code ?? 200, response);
    }
    // Deletes a medical record based on the provided details.
    // Parameters:
    // - deleteDto: An object containing the identifier of the medical record to delete.
    // Returns: A boolean indicating whether the deletion was successful.
    [HttpDelete]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteMedicalRecord(
        [FromBody] DeleteMedicalDto deleteDto)
    {
        var response = await _medicalRecordService.DeleteMedicalRecordAsync(deleteDto);
        return StatusCode(response.Code ?? 200, response);
    }
}