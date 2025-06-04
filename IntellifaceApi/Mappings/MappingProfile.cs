using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.Entities.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeTrainImage, TrainImageCreateDto>().ReverseMap();
        CreateMap<EmployeeTrainImage, TrainImageDto>().ReverseMap();
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Attendance, AttendanceDto>().ReverseMap();
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Location, LocationDto>().ReverseMap();
    }
}