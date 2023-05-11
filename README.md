# RSBingoBot

## About This Project

RSBingoBot is a containerized application designed to provide (TODO: provide a brief description of what the bot does). This repository contains the application's source code, Dockerfile for containerization, and instructions for building and running the application.

## Prerequisites

Before proceeding, ensure you have the following software installed:

- [Docker](https://www.docker.com/products/docker-desktop): Download and install the latest version of Docker Desktop for your operating system.

## Building the Docker Image

Follow these steps to build the Docker image for RSBingoBot:

1. Open a terminal (Command Prompt, PowerShell, or a Unix-based terminal).
2. Navigate to the root folder of the RSBingoBot repository where the `Dockerfile` is located.
3. Run the following command to build the Docker image:

```bash
docker build -t rsbingobot:1.X.X .
```


## Running the Application

To run the application in a Docker container, create a docker-compose.yml file with your specific details, example:

```yaml

version: "3.9"
services:
  rsbingobot:
    image: rsbingobot:1.X.X
    container_name: rsbingobot_container
    environment:
      - TZ=<YourTimeZone>
    restart: on-failure
    network_mode: "host"
    volumes:
      - "<HostPath>/appsettings.Production.json:/app/appsettings.Production.json"
```

Note: Replace <YourTimeZone> and <HostPath> with appropriate values. As well as 1.X.X with the correct version.

The appsettings.Production.json should look like the following

```json

{
  "PointsForEasyTile": 1,
  "PointsForMediumTile": 2,
  "PointsForHardTile": 3,
  "BonusPointsForEasyCompletion": 5,
  "BonusPointsForMediumCompletion": 10,
  "BonusPointsForHardCompletion": 15,
  "BonusPointsForRow": 2,
  "BonusPointsForColumn": 4,
  "BonusPointsForDiagonal": 4,

  "EnableBoardCustomisation": false,

  "ConnectionStrings": {
    "DB": "Server=xxx;Database=xxx;Uid=xxx;Pwd=xxx",
    "Schema": "xxx"
  },

  "Serilog": {
    "MinimumLevel": "Debug",
    "WriteTo": [
      {
        "Name": "Console",
        "Args": { "restrictedToMinimumLevel": "Information" }
      },
      {
        "Name": "File",
        "Args": {
          "path": "dev logs/.log",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
          "restrictedToMinimumLevel": "Debug",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true,
          "pathFormat": "yyyy-MM-dd"
        }
      }
    ]
  },

  "BotToken": YourBotToken,
  "GuildId": ID,

  "UseNpgsql" : false, // Defaults to mySQL when false, uses postgresql when true.

  "PendingEvidenceChannelId": ID,
  "VerifiedEvidenceChannelId": ID,
  "RejectedEvidenceChannelId": ID,
  "LeaderboardChannelId": ID,
}

```

After creating and configuring the docker-compose.yml file, run the following command to deploy the container:

```bash
docker-compose up -d
```

## Generating a Coverage Report

To generate a coverage report, navigate to the /RS-Bingo-Bot/RSBingo-Framework-Tests directory and run the following command:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='./Coverage/coverage'
```

We use the OpenCover format to enable usage of tools like SonarQube.