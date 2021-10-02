pipeline {
  agent any
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

  }
}
