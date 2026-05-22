FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "CinemaAPI/CinemaAPI.csproj"

RUN dotnet publish "CinemaAPI/CinemaAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "CinemaAPI.dll", "--urls", "http://0.0.0.0:5000"]