using Backend.DTOs;
using Backend.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Backend.Validations
{
    // This class validates the update operations for medical records based on the UpdateMedicalDto. 
    public class UpdateMedicalDtoValidator : BaseValidator<UpdateMedicalDto>
    {
        public UpdateMedicalDtoValidator(HRDbContext context) : base(context)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            When(x => x.StartDate.HasValue, () => {
                RuleFor(x => x.EndDate)
                    .Must((dto, endDate) => !endDate.HasValue || endDate.Value > dto.StartDate.Value)
                    .WithMessage("End Date must be after Start Date");
            });

            When(x => x.StatusId == 2, () => {
                RuleFor(x => x.DeletionReason)
                    .NotEmpty().WithMessage("Deletion Reason is required for Inactive status")
                    .MaximumLength(2000).WithMessage("Deletion Reason cannot exceed 2000 characters");

                RuleFor(x => x.EndDate)
                    .NotNull().WithMessage("End Date is required for Inactive status");

                RuleFor(x => x.ModifiedBy)
                    .NotEmpty().WithMessage("Modified By is required for status change");
            });

            RuleFor(x => x)
                .CustomAsync(async (dto, context, cancellation) => {
                    var existingRecord = await _context.t_medical_records
                        .FirstOrDefaultAsync(m => m.medical_record_id == dto.Id);

                    if (existingRecord?.status_id == 2)
                    {
                        context.AddFailure("StatusId", "Cannot modify an Inactive record");
                    }

                    if (dto.EndDate.HasValue && dto.StatusId != 2)
                    {
                        context.AddFailure("StatusId", "Record with End Date must be set to Inactive status");
                    }
                });
        }
    }
}