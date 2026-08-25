FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=true \
    MSBUILDSINGLELOADCONTEXT=1
COPY ["HospitalManagementSystem.sln", "./"]
COPY ["src/HospitalManagement.Domain/HospitalManagement.Domain.csproj", "src/HospitalManagement.Domain/"]
COPY ["src/HospitalManagement.Application/HospitalManagement.Application.csproj", "src/HospitalManagement.Application/"]
COPY ["src/HospitalManagement.Infrastructure/HospitalManagement.Infrastructure.csproj", "src/HospitalManagement.Infrastructure/"]
COPY ["src/HospitalManagement.Web/HospitalManagement.Web.csproj", "src/HospitalManagement.Web/"]
RUN dotnet restore "src/HospitalManagement.Web/HospitalManagement.Web.csproj"
COPY . .
RUN rm -f obj/project.assets.json obj/project.nuget.cache && dotnet publish "src/HospitalManagement.Web/HospitalManagement.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends wget \
    && rm -rf /var/lib/apt/lists/*
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_EnableDiagnostics=0
COPY --from=build /app/publish .
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "HospitalManagement.Web.dll"]
