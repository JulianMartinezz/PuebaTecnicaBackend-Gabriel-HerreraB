using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.DTOs
{
    // DTO used for creating a new medical record. Inherits from BaseMedicalRecordDto 
    // and includes additional properties like CreatedBy and FileId.
    public class CreateMedicalDto : BaseMedicalRecordDto
    {
        public string CreatedBy { get; set; }
        public int FileId { get; set; }
        public DateOnly? CreationDate { get; set; }
    }
}
