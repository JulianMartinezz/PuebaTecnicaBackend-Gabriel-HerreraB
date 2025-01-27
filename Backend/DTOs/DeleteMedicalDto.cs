namespace Backend.DTOs
{
    // DTO used for logically deleting a medical record. 
    // Contains properties to identify the record, the user performing the deletion, 
    // and the reason for deletion.
    public class DeleteMedicalDto
    {
        public int Id { get; set; }
        public int MedicalRecordId { get; set; }
        public string DeletedBy { get; set; }
        public string DeletionReason { get; set; }
    }
}
