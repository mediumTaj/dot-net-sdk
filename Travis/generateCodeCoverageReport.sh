#! /bin/sh
Test/packages/OpenCover.4.6.519/tools/OpenCover.Console.exe -target:Test/packages/NUnit.ConsoleRunner.3.5.0/tools/nunit3-console.exe -targetargs:Test/Test/bin/Release/Test.dll -filter:"+[*]*" -register:user
if [ $? = 0 ] ; then
  echo 'Coverage report was generated.'
else
  echo 'Failed to generate code coverage report.'
fi
