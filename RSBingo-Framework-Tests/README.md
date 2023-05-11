# Testing

## Generating a Coverage Report

To generate a coverage report, navigate to the /RS-Bingo-Bot/RSBingo-Framework-Tests directory and run the following command:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='./Coverage/coverage'
```

We use the OpenCover format to enable usage of tools like SonarQube.


## SonarQube

### Rrerequisites

1. Install Java 17 

2. Install sonarscanner, which can be done using the following command

```bash
dotnet tool install --global dotnet-sonarscanner 
```

### Installing 

Donwload the link from the following https://binaries.sonarsource.com/Distribution/sonarqube/sonarqube-10.0.0.68432.zip

Execute your OS spesific StartSonar with bin/OS/StartSonar.xyz

i.e /bin/windows-x86-64/StartSonar.bat on windows


### Setting up the project for the first time

Go to: Go to: http://localhost:9000/

Login with the default credentials, you will then be prompted to enter a new password.

Add a new manual project

Generate your Key and secret token. Keep these in a scret place.

Run in the terminal

```bash
dotnet sonarscanner begin /k:"<YourKey>" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="<YourToken>"

dotnet build

dotnet sonarscanner end /d:sonar.login="<YourToken>"

```

This will be your inital import.


### Adding coverage report and code changes

Every time you SonarQube to run a report, do the following.

1. Make sure the project compiles and all unit tests pass. Use the "[Ignore]" attribute on failing tests if you cannot get it to pass.
2. Run a coverage report as descirbed in: Generating a Coverage Report
3. Run the following scripts

```bash

dotnet sonarscanner begin /k:"<YourKey>" /d:sonar.login="<YourToken>" /d:sonar.host.url="http://localhost:9000" /d:sonar.cs.opencover.reportsPaths="RSBingo-Framework-Tests/Coverage/coverage.opencover.xml"

dotnet build

dotnet sonarscanner end /d:sonar.login="<YourToken>"
```