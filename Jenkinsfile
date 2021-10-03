
pipeline {
  agent any
  environment {
    scannerHome = tool 'SonarQubeMsBuild'
  }
  stages {
    stage('Begin SonarQube analysis') {
      when {
        branch 'master'
      }
      steps {
        sh "dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools"
        withSonarQubeEnv(installationName: 'SonarQube_Convertara', credentialsId: 'SonarQubeToken') {
          sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"Convertara\" /d:sonar.coverageReportPaths=\"**/SonarQube.xml\" /d:sonar.cs.vstest.reportsPaths=\"**/*.trx\""
        }
      }
    }
    stage('Build') {
      steps {
        dotnetBuild(project: 'src/Convertara.Core/Convertara.Core.csproj')
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test src/Convertara.Test/Convertara.Test.csproj --collect:"XPlat Code Coverage" --logger trx'
      }
    }

    stage('Convert coverage to SonarQube') {
      steps {
        sh './tools/reportgenerator "-reports:src/Convertara.Test/TestResults/*/coverage.cobertura.xml" "-targetdir:sonarqubecoverage" "-reporttypes:SonarQube"'
      }
    }

    stage('Publish Coverage Report') {
      steps {
        cobertura(coberturaReportFile: '**/coverage.cobertura.xml')
        xunit(
          [MSTest(deleteOutputFiles: true,
                  failIfNotNew: true,
                  pattern: 'src/Convertara.Test/TestResults/*.trx',
                  skipNoTestFiles: false,
                  stopProcessingIfError: true)
          ])
      }
    }

    stage('SonarQube Analysis') {
      when {
        branch 'master'
      }
      steps {
        withSonarQubeEnv(installationName: 'SonarQube_Convertara', credentialsId: 'SonarQubeToken') {
          //sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"Convertara\""
          //sh "dotnet build src/Convertara.Core/Convertara.Core.csproj"
          sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
        }
      }
    }
  }
}
