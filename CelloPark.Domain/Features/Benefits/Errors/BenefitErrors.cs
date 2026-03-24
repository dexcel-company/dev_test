using CelloPark.Domain.Common.Errors;
using ErrorOr;

namespace CelloPark.Domain.Features.Benefits.Errors;

public static class BenefitErrors
{
    public static Error NameIsNull => Error.Validation(
        code: BenefitErrorCodes.Name,
        description: string.Format(ErrorDescriptions.Null, nameof(Benefit.Name)));

    public static Error NameIsTooShort => Error.Validation(
        code: BenefitErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooShort, nameof(Benefit.Name)));

    public static Error NameIsTooLong => Error.Validation(
        code: BenefitErrorCodes.Name,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Benefit.Name)));

    public static Error DescriptionIsTooLong => Error.Validation(
        code: BenefitErrorCodes.Description,
        description: string.Format(ErrorDescriptions.TooLong, nameof(Benefit.Description)));

    public static Error StartActiveDateIsInvalid => Error.Validation(
        code: BenefitErrorCodes.StartActiveDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.StartActiveDate)));

    public static Error StartActiveDateIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.StartActiveDate,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.StartActiveDate)));

    public static Error EndActiveDateIsInvalid => Error.Validation(
        code: BenefitErrorCodes.EndActiveDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.EndActiveDate)));

    public static Error EndActiveDateIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.StartActiveDate,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.EndActiveDate)));

    public static Error StartPromotionDateIsInvalid => Error.Validation(
        code: BenefitErrorCodes.StartPromotionDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.StartPromotionDate)));

    public static Error StartPromotionDateIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.StartActiveDate,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.StartPromotionDate)));

    public static Error EndPromotionDateIsInvalid => Error.Validation(
        code: BenefitErrorCodes.EndPromotionDate,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.EndPromotionDate)));

    public static Error EndPromotionDateIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.EndPromotionDate,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.EndPromotionDate)));

    public static Error ActivationDateDurationIsInvalid => Error.Validation(
        code: BenefitErrorCodes.ActivationDateDuration,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.Duration)));

    public static Error ActivationDateDurationIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.ActivationDateDuration,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.Duration)));

    public static Error CouponDateDurationIsInvalid => Error.Validation(
        code: BenefitErrorCodes.CouponDateDuration,
        description: string.Format(ErrorDescriptions.Invalid, nameof(Benefit.CouponsDuration)));

    public static Error CouponDateDurationIsNotUtc => Error.Validation(
        code: BenefitErrorCodes.CouponDateDuration,
        description: string.Format(ErrorDescriptions.NotUtc, nameof(Benefit.CouponsDuration)));

    public static Error NameAlreadyExists => Error.Conflict(
        code: BenefitErrorCodes.Name,
        description: "Benefit with the same name already exists.");

    public static Error NotFound => Error.NotFound(
        code: BenefitErrorCodes.NotFound,
        description: string.Format(ErrorDescriptions.NotFound, nameof(Benefit)));
}
