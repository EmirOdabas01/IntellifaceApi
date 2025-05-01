using AutoMapper;
using Intelliface.DTOs;
using Intelliface.DTOs.Attendance;
using Intelliface.DTOs.Department;
using Intelliface.DTOs.Employee;
using Intelliface.DTOs.Location;
using Intelliface.Entities.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeReadDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        CreateMap<EmployeeCreateDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>();
        CreateMap<Employee, EmployeeUpdateDto>();

        CreateMap<Department, DepartmentReadDto>()
      .ForMember(dest => dest.LocationAddress, opt => opt.MapFrom(src => src.Location.Address));

        CreateMap<DepartmentCreateDto, Department>();
        CreateMap<DepartmentUpdateDto, Department>();

        CreateMap<Location, LocationReadDto>();
        CreateMap<LocationCreateDto, Location>();
        CreateMap<LocationUpdateDto, Location>();

        CreateMap<Attendance, AttendanceReadDto>()
    .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee.Name + " " + src.Employee.Surname));

        CreateMap<AttendanceCreateDto, Attendance>();
        CreateMap<AttendanceUpdateDto, Attendance>();

    }
}