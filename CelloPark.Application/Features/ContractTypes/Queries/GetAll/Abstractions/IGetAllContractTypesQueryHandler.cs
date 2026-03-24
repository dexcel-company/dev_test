using CelloPark.Application.Common.Attributes;
using CelloPark.Application.Common.Pagination;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.ContractTypes.Queries.GetAll.Abstractions;

[SingletonHandler]
public interface IGetAllContractTypesQueryHandler
{
    Page<ContractType> Handle(GetAllContractTypesQuery request);
}
