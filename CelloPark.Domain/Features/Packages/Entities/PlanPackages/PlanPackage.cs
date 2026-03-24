using CelloPark.Domain.Common.Enums.Abstractions;
using CelloPark.Domain.Common.Enums.Statuses;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Common.ValueObjects.Abstractions;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Create;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Delete;
using CelloPark.Domain.Common.ValueObjects.AuditDetails.Update;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages.Constants;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages.Errors;
using CelloPark.Domain.Features.Plans;
using ErrorOr;

namespace CelloPark.Domain.Features.Packages.Entities.PlanPackages;

public sealed class PlanPackage :
    ICreateDetailsOwner, IUpdateDetailsOwner, IDeleteDetailsOwner, IStatusOwner
{
    private PlanPackage() { }

    private PlanPackage(
        long planId,
        long packageId,
        decimal price,
        int vat)
    {
        PlanId = planId;
        PackageId = packageId;
        Price = price;
        Vat = vat;
        Status = Status.Active;
    }
    public long PlanId { get; private set; }
    public Plan Plan { get; private set; } = null!;
    public long PackageId { get; private set; }
    public Package Package { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Vat { get; private set; }
    public Status Status { get; private set; }
    public CreateDetails CreateDetails { get; private set; } = null!;
    public UpdateDetails UpdateDetails { get; private set; } = null!;
    public DeleteDetails DeleteDetails { get; private set; } = null!;

    public static ErrorOr<PlanPackage> Create(
        long planId,
        long packageId,
        decimal price,
        int vat)
    {
        ErrorOr<long> planIdResult = ValidatePlanId(planId);
        ErrorOr<long> packageIdResult = ValidatePackageId(packageId);
        ErrorOr<decimal> priceResult = ValidatePrice(price);
        ErrorOr<int> vatResult = ValidateVat(vat);

        List<Error> errors = ErrorProvider.Join(
            planIdResult,
            packageIdResult,
            priceResult,
            vatResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new PlanPackage(
            planId: planIdResult.Value,
            packageId: packageIdResult.Value,
            price: priceResult.Value,
            vat: vatResult.Value);
    }

    public ErrorOr<None> UpdatePrice(decimal price)
    {
        ErrorOr<decimal> priceResult = ValidatePrice(price);

        if (priceResult.IsError)
        {
            return priceResult.FirstError;
        }

        Price = priceResult.Value;

        return None.Value;
    }

    public ErrorOr<None> UpdateVat(int vat)
    {
        ErrorOr<int> vatResult = ValidateVat(vat);

        if (vatResult.IsError)
        {
            return vatResult.FirstError;
        }

        Vat = vatResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddCreateDetails(DateTime? createdAt, Guid? createdBy)
    {
        ErrorOr<CreateDetails> createDetailsResult = CreateDetails.Create(createdAt, createdBy);

        if (createDetailsResult.IsError)
        {
            return createDetailsResult.Errors;
        }

        CreateDetails = createDetailsResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddUpdateDetails(DateTime? updatedAt, Guid? updatedBy)
    {
        ErrorOr<UpdateDetails> updateDetailsResult = UpdateDetails.Create(updatedAt, updatedBy);

        if (updateDetailsResult.IsError)
        {
            return updateDetailsResult.Errors;
        }

        UpdateDetails = updateDetailsResult.Value;

        return None.Value;
    }

    public ErrorOr<None> AddDeleteDetails(DateTime? deletedAt, Guid? deletedBy)
    {
        ErrorOr<DeleteDetails> deleteDetailsResult = DeleteDetails.Create(deletedAt, deletedBy);

        if (deleteDetailsResult.IsError)
        {
            return deleteDetailsResult.Errors;
        }

        DeleteDetails = deleteDetailsResult.Value;

        return None.Value;
    }

    public void MarkAsDeleted()
    {
        Status = Status.Deleted;
    }

    public void MarkAsActive()
    {
        Status = Status.Active;
    }

    public void MarkAsInactive()
    {
        Status = Status.Inactive;
    }

    private static ErrorOr<long> ValidatePlanId(long planId)
    {
        if (planId == default)
        {
            return PlanPackageErrors.PlanIdIsInvalid;
        }

        return planId;
    }

    private static ErrorOr<long> ValidatePackageId(long packageId)
    {
        if (packageId == default)
        {
            return PlanPackageErrors.PackageIdIsInvalid;
        }

        return packageId;
    }

    private static ErrorOr<decimal> ValidatePrice(decimal price)
    {
        if (price < PlanPackageSettings.PriceMinValue)
        {
            return PlanPackageErrors.PriceIsTooSmall;
        }

        if (price > PlanPackageSettings.PriceMaxValue)
        {
            return PlanPackageErrors.PriceIsTooBig;
        }

        return price;
    }

    private static ErrorOr<int> ValidateVat(int vat)
    {
        if (vat < PlanPackageSettings.VatMinValue)
        {
            return PlanPackageErrors.VatIsTooSmall;
        }

        if (vat > PlanPackageSettings.VatMaxValue)
        {
            return PlanPackageErrors.VatIsTooBig;
        }

        return vat;
    }
}
