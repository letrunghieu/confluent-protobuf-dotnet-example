FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConfluentProtobufDotnetExample/ConfluentProtobufDotnetExample.csproj", "ConfluentProtobufDotnetExample/"]
RUN dotnet restore "ConfluentProtobufDotnetExample/ConfluentProtobufDotnetExample.csproj"
COPY . .
WORKDIR "/src/ConfluentProtobufDotnetExample"
RUN dotnet build "ConfluentProtobufDotnetExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfluentProtobufDotnetExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfluentProtobufDotnetExample.dll"]
