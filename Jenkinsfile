
pipeline {
  agent any
  environment {
    scannerHome = tool 'SonarQubeMsBuild'
  }
  stages {
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
        sh "echo ${scannerHome}"
        withSonarQubeEnv(installationName: 'SonarQube_Convertara', credentialsId: 'SonarQubeToken') {
          sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"Convertara\""
          sh "dotnet build src/Convertara.Core/Convertara.Core.csproj"
          sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
        }
      }
    }
  }
}
