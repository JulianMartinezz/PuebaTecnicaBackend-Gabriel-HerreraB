namespace Backend.DTOs
{
    // DTO used for updating an existing medical record. 
    // Inherits from BaseMedicalRecordDto and adds properties like the record ID, 
    // the user who modified it, and the reason for deletion if applicable.
    public class UpdateMedicalDto : BaseMedicalRecordDto
    {
        public int Id { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletionReason { get; set; }
        public DateOnly? ModificationDate { get; set; }

    }
}
