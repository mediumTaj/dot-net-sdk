#! /bin/sh
# OpenCover does not work on Linux. Do this locally on build!
Test/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe -target:Test/packages/NUnit.ConsoleRunner.3.5.0/tools/nunit3-console.exe -targetargs:Test/Test/bin/Release/Test.dll -excludebyfile:*DataModels.cs -hideskipped:all -filter:"+[*]* -[Test]* -[sdk]FullSerializer.* -[sdk]MiniJSON.*" -register:user
if [ $? = 0 ] ; then
  echo 'Coverage report was generated.'
  #generate human readable report
  Test/packages/ReportGenerator.2.5.1/tools/ReportGenerator.exe -reports:results.xml -targetdir:"reports" -reporttype:HtmlSummary
  if [ $? = 0 ] ; then
    echo 'Human readable report was generated.'
    exit 0
  else

  fi
else
  echo 'Failed to generate code coverage report.'
  exit 1
fi
