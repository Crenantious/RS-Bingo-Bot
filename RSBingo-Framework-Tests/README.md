# Testing

## Generating a Coverage Report

To generate a coverage report, navigate to the /RS-Bingo-Bot/RSBingo-Framework-Tests directory and run the following command:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput='./Coverage/coverage'
```

We use the OpenCover format to enable usage of tools like SonarQube.


## SonarQube

### Prerequisites

1. Install Java 17 .

2. Install sonarscanner, which can be done using the following command:

```bash
dotnet tool install --global dotnet-sonarscanner 
```

### Installing 

Download the following: https://binaries.sonarsource.com/Distribution/sonarqube/sonarqube-10.0.0.68432.zip

Execute your OS specific StartSonar with bin/OS/StartSonar.xyz

I.e `/bin/windows-x86-64/StartSonar.bat on Windows`


### Setting up the project for the first time

1. Go to: `http://localhost:9000/`.

2. Login with the default credentials, you will then be prompted to enter a new password.

3. Add a new manual project.

4. Generate your key and secret token. Keep these in a secret place.

5. Run the following:

```bash
dotnet sonarscanner begin /k:"<YourKey>" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="<YourToken>"

dotnet build

dotnet sonarscanner end /d:sonar.login="<YourToken>"

```

This will be your inital import.


### Adding coverage reports and code changes

Every time you want to generate a new SonarQube report, do the following:

1. Make sure the project compiles and all unit tests pass. Use the "[Ignore]" attribute on failing tests if you cannot get them to pass.

2. Run a coverage report as described in: Generating a Coverage Report.

3. Run the following:

```bash

dotnet sonarscanner begin /k:"<YourKey>" /d:sonar.login="<YourToken>" /d:sonar.host.url="http://localhost:9000" /d:sonar.cs.opencover.reportsPaths="RSBingo-Framework-Tests/Coverage/coverage.opencover.xml"

dotnet build

dotnet sonarscanner end /d:sonar.login="<YourToken>"
```
