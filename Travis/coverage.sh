#!/bin/bash

set -e

if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  echo '$TRAVIS_PULL_REQUEST is false, running tests'

  echo "Moving appdata directory..."
  mv Test/Test/appdata/ Test/Test/bin/Release/appdata/

  echo "Decrypting config..."
  openssl aes-256-cbc -K $encrypted_89fb597d004e_key -iv $encrypted_89fb597d004e_iv -in Config.json.enc -out Test/Test/bin/Release/appdata/Config.json -d

  #echo "Running dot-net-sdk integration Tests..."
  #mono ./NUnit.ConsoleRunner.3.5.0/tools/nunit3-console.exe Test/Test/bin/Release/Test.dll --result=reports/TestResults.xml

  # Install OpenCover and ReportGenerator, and save the path to their executables.
  nuget install -Verbosity quiet -OutputDirectory Test/packages -Version 4.6.519 OpenCover
  nuget install -Verbosity quiet -OutputDirectory Test/packages -Version 2.4.5.0 ReportGenerator

  OPENCOVER=Test/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe
  REPORTGENERATOR=Test/packages/ReportGenerator.2.4.5.0/tools/ReportGenerator.exe

  echo "opencover: " + $OPENCOVER
  echo "current directory: " $PWD

  # echo "Attempting mono call for opencover"
  # mono ./Test/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe -target:"Test/packages/NUnit.ConsoleRunner.3.5.0/tools/nunit3-console.exe" -targetargs:"Test/Test/bin/Release/Test.dll -result=reports/TestResults.xml" -output:reports/results.xml -mergeoutput -hideskipped:File -oldStyle -skipautoprops -excludebyfile:*DataModels.cs -filter:"+[*]* -[Test]* -[sdk]FullSerializer.* -[sdk]MiniJSON.*" -register:user
  #
  # echo "Attempting mono call for report generator"
  chmod +x $OPENCOVER
  chmod +x $REPORTGENERATOR
  echo "Calculating coverage with OpenCover..."
  $OPENCOVER \
    -target:"Test/packages/NUnit.ConsoleRunner.3.5.0/tools/nunit3-console.exe" \
    -targetargs:"Test/Test/bin/Release/Test.dll --result=reports/TestResults.xml" \
    -output:reports/results.xml \
    -mergeoutput \
    -hideskipped:File \
    -oldStyle \
    -skipautoprops \
    -excludebyfile:*DataModels.cs \
    -filter:"+[*]* -[Test]* -[sdk]FullSerializer.* -[sdk]MiniJSON.*" \
    -register:user

  echo "Generating HTML report..."
  $REPORTGENERATOR \
    -reports:reports/results.xml \
    -targetdir:"reports" \
    -verbosity:Error

else
  echo '$TRAVIS_PULL_REQUEST is not false ($TRAVIS_PULL_REQUEST), skipping tests'
fi
