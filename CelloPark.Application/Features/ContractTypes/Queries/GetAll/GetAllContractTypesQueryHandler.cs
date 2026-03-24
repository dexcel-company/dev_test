using CelloPark.Application.Common.Pagination;
using CelloPark.Application.Common.Pagination.Extensions;
using CelloPark.Application.Features.ContractTypes.Queries.GetAll.Abstractions;
using CelloPark.Domain.Common.Enums.ContractTypes;

namespace CelloPark.Application.Features.ContractTypes.Queries.GetAll;

internal sealed class GetAllContractTypesQueryHandler :
    IGetAllContractTypesQueryHandler
{
    public Page<ContractType> Handle(GetAllContractTypesQuery request)
    {
        Page<ContractType> contractTypePage = ContractType.Elements
            .ApplyPagination(request.PaginationCriteria);

        return contractTypePage;
    }
}
