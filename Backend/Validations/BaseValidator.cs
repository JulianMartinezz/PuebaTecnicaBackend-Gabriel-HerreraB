using Backend.DTOs;
using Backend.Models;
using FluentValidation;

namespace Backend.Validations
{
    // This is an abstract base validator class that provides common validation rules for medical record DTOs.
    // It ensures that fields adhere to length and format restrictions,
    // and also applies conditional validations based on the values of certain fields.
    public abstract class BaseValidator<T> : AbstractValidator<T> where T : BaseMedicalRecordDto
    {
        protected readonly HRDbContext _context;

        protected BaseValidator(HRDbContext context)
        {
            _context = context;
            AddCommonValidations();
        }

        protected void AddCommonValidations()
        {
            RuleFor(x => x.Diagnosis)
                .MaximumLength(100).WithMessage("Diagnosis cannot exceed 100 characters");

            RuleFor(x => x.MotherData)
                .MaximumLength(2000).WithMessage("Mother Data cannot exceed 2000 characters");

            RuleFor(x => x.FatherData)
                .MaximumLength(2000).WithMessage("Father Data cannot exceed 2000 characters");

            RuleFor(x => x.OtherFamilyData)
                .MaximumLength(2000).WithMessage("Other Family Data cannot exceed 2000 characters");

            RuleFor(x => x.MedicalBoard)
                .MaximumLength(200).WithMessage("Medical Board cannot exceed 200 characters");

            RuleFor(x => x.Observations)
                .MaximumLength(2000).WithMessage("Observations cannot exceed 2000 characters")
                .NotEmpty().When(x => x.PositionChange == "YES")
                .WithMessage("Observations are required when Position Change is YES");

            RuleFor(x => x.Audiometry)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Audiometry must be 'YES' or 'NO'");

            RuleFor(x => x.PositionChange)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Position Change must be 'YES' or 'NO'");

            RuleFor(x => x.ExecuteMicros)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Execute Micros must be 'YES' or 'NO'");

            RuleFor(x => x.ExecuteExtra)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Execute Extra must be 'YES' or 'NO'");

            RuleFor(x => x.VoiceEvaluation)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Voice Evaluation must be 'YES' or 'NO'");

            RuleFor(x => x.Disability)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Disability must be 'YES' or 'NO'");

            RuleFor(x => x.AreaChange)
                .Must(x => x == null || x == "YES" || x == "NO")
                .WithMessage("Area Change must be 'YES' or 'NO'");

            RuleFor(x => x.DisabilityPercentage)
                .Must((dto, percentage) =>
                    dto.Disability != "YES" ||
                    (percentage.HasValue && percentage.Value >= 0 && percentage.Value <= 100))
                .When(x => x.Disability == "YES")
                .WithMessage("Disability Percentage must be between 0 and 100 when Disability is 'YES'");
        }
    }
}
