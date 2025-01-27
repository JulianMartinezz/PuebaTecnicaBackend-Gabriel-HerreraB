using Backend.DTOs;
using Backend.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Backend.Validations
{
    // This class validates the creation of a new medical record based on the CreateMedicalDto. 
    // It inherits from BaseValidator and adds specific rules for required fields
    public class CreateMedicalDtoValidator : BaseValidator<CreateMedicalDto>
    {
        public CreateMedicalDtoValidator(HRDbContext context) : base(context)
        {
            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Start Date cannot be in the future");

            RuleFor(x => x.StatusId)
                .NotNull().WithMessage("Status ID is required")
                .Must(statusId => statusId != 2)
                .WithMessage("Cannot create record with Inactive status")
                .MustAsync(async (statusId, cancellation) =>
                    await _context.statuses.AnyAsync(s => s.status_id == statusId))
                .WithMessage("Invalid Status ID");

            RuleFor(x => x.MedicalRecordTypeId)
                .NotNull().WithMessage("Medical Record Type ID is required")
                .MustAsync(async (typeId, cancellation) =>
                    await _context.medical_record_types.AnyAsync(m => m.medical_record_type_id == typeId))
                .WithMessage("Invalid Medical Record Type ID");

            RuleFor(x => x.FileId)
                .NotNull().WithMessage("File ID is required");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("Created By is required")
                .MaximumLength(2000).WithMessage("Created By cannot exceed 2000 characters");
        }
    }
}