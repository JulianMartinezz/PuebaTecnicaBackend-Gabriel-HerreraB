namespace Backend.DTOs
{
    // DTO used for retrieving medical record details. 
    // Extends BaseMedicalRecordDto and includes additional properties such as 
    // record ID, created/modified/deleted information, and timestamps.
    public class GetMedicalDto : BaseMedicalRecordDto
    {
        public int MedicalRecordId { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateOnly? CreationDate { get; set; }
        public DateOnly? ModificationDate { get; set; }
        public DateOnly? DeletionDate { get; set; }
    }
    
}
