#! /bin/sh
echo "Attempting to restore packages!"
nuget restore Test/Test.sln
if [ $? = 0 ] ; then
  echo "Attempting to build sdk!"
  xbuild /p:Configuration=Release sdk/sdk.sln
  if [ $? = 0 ] ; then
    echo "SDK build succeeded!"
    echo "Attempting to build Tests"
    xbuild /p:Configuration=Release Test/Test.sln
    if [ $? = 0 ] ; then
      echo "Test build succeeded!"
      exit 0
    else
      echo "Failed to build Test"
      exit 1
    fi
  else
    echo "Failed to build sdk!"
    exit 1
  fi
else
  echo "Failed to restore packages!"
  exit 1
fi
