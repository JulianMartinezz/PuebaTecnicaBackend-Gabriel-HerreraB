using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DTOs
{
    // Defines the base properties shared across all medical record-related DTOs, 
    // such as diagnosis, start and end dates, status and type identifiers and names, 
    // observations, and family data.

    public abstract class BaseMedicalRecordDto
    {
        public string? Diagnosis { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? MedicalRecordTypeId { get; set; }
        public string? MedicalRecordTypeName { get; set; }
        public string? Observations { get; set; }
        public string? MotherData { get; set; }
        public string? FatherData { get; set; }
        public string? OtherFamilyData { get; set; }
        public string? MedicalBoard { get; set; }
        public string? Audiometry { get; set; }
        public string? ExecuteMicros { get; set; }
        public string? ExecuteExtra { get; set; }
        public string? VoiceEvaluation { get; set; }
        public string? AreaChange { get; set; }
        public string? Disability { get; set; }
        public int? DisabilityPercentage { get; set; }
        public string? PositionChange { get; set; }
    }
}