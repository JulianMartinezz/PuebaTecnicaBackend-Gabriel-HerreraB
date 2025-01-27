using Backend.DTOs;
using Backend.Models;
using FluentValidation;

namespace Backend.Validations
{
    // This class validates the data for deleting a medical record based on the DeleteMedicalDto.
    public class DeleteMedicalDtoValidator : AbstractValidator<DeleteMedicalDto>
    {
        public DeleteMedicalDtoValidator()
        {
            RuleFor(x => x.DeletionReason)
                .NotEmpty().WithMessage("Deletion Reason is required")
                .MaximumLength(2000).WithMessage("Deletion Reason cannot exceed 2000 characters");

            RuleFor(x => x.DeletedBy)
                .NotEmpty().WithMessage("Deleted By is required")
                .MaximumLength(2000).WithMessage("Deleted By cannot exceed 2000 characters");
        }
    }
}