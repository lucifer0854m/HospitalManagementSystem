using AutoMapper;
using HospitalManagement.Application.Mapping;

namespace HospitalManagement.Tests;

internal static class TestHelpers
{
    public static IMapper Mapper => new MapperConfiguration(config => config.AddProfile<ApplicationMappingProfile>()).CreateMapper();
}
