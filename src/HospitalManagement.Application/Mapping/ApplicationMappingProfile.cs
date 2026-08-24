using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(d => d.FullName,
                o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.Gender,
                o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.Status,
                o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.BloodGroup,
                o => o.MapFrom(s => s.BloodGroup.ToString()));

        CreateMap<CreatePatientDto, Patient>();

        CreateMap<UpdatePatientDto, Patient>();

        CreateMap<Patient, UpdatePatientDto>();

        CreateMap<Doctor, DoctorDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department == null ? string.Empty : s.Department.Name));

        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>();

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient == null ? string.Empty : $"{s.Patient.FirstName} {s.Patient.LastName}"))
            .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor == null ? string.Empty : $"{s.Doctor.FirstName} {s.Doctor.LastName}"))
            .ForMember(d => d.AppointmentType, o => o.MapFrom(s => s.AppointmentType.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<CreateAppointmentDto, Appointment>();
        CreateMap<UpdateAppointmentDto, Appointment>();

        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();
    }
}
