using CelloPark.Application.Features.Items.Dtos;
using CelloPark.Domain.Common.Enums.ContractTypes;
using CelloPark.Domain.Common.Errors;
using CelloPark.Domain.Common.Results;
using CelloPark.Domain.Features.Items;
using ErrorOr;

namespace CelloPark.Application.Features.Items.Extensions;

public static class ItemExtensions
{
    public static ErrorOr<Item> ToModel(this ItemCreateDto dto)
    {
        ErrorOr<Item> itemResult = Item.Create(
            shadowId: dto.ShadowId,
            name: dto.Name,
            description: dto.Description,
            contractType: ContractType.FromKey(dto.ContractType));

        if (itemResult.IsError)
        {
            return itemResult.Errors;
        }

        return itemResult.Value;
    }

    public static ErrorOr<Item> Update(this Item model, ItemUpdateDto dto)
    {
        ContractType? contractType = ContractType.FromKey(dto.ContractType);

        ErrorOr<None> shadowIdResult = model.UpdateShadowId(dto.ShadowId);
        ErrorOr<None> nameResult = model.UpdateName(dto.Name);
        ErrorOr<None> descriptionResult = model.UpdateDescription(dto.Description);
        ErrorOr<None> contractTypeIdResult = model.UpdateContractType(contractType);

        List<Error> errors = ErrorProvider.Join(
            shadowIdResult,
            nameResult,
            descriptionResult,
            contractTypeIdResult);

        if (errors.Count > 0)
        {
            return errors;
        }

        return model;
    }
}
