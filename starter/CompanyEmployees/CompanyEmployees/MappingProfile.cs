using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace CompanyEmployees;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
          .ForMember((CompanyDto cDto) => cDto.FullAddress,
             (IMemberConfigurationExpression<Company, CompanyDto, string> opt) => 
                opt.MapFrom((Company c) => string.Join(' ', c.Address, c.Country)));
        
        CreateMap<Employee, EmployeeDto>(); // TSource -> TDestination

        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

        CreateMap<CompanyForUpdateDto, Company>();
    }
}
