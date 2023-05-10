# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Using the ASP.NET Core runtime image which includes both .NET runtime and ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RSBingoBot/RSBingoBot.csproj", "RSBingoBot/"]
COPY ["RSBingo-Common/RSBingo-Common.csproj", "RSBingo-Common/"]
COPY ["RSBingo-Framework/RSBingo-Framework.csproj", "RSBingo-Framework/"]
RUN dotnet restore "RSBingoBot/RSBingoBot.csproj"
COPY . .
WORKDIR "/src/RSBingoBot"
RUN dotnet build "RSBingoBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RSBingoBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RSBingoBot.dll"]