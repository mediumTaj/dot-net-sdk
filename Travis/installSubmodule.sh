echo "Attempting to install the base .NET SDK into the sdk..."

git clone -b feature-dotNetSDKChanges https://github.com/mediumTaj/csharp-sdk-base.git /home/travis/build/mediumTaj/dot-net-sdk/sdk/src/services
if [ $? = 0 ] ; then
  echo "Base .NET SDK install SUCCEEDED! Exited with $?"
  exit 0
else
  echo "Base .NET SDK install FAILED! Exited with $?"
  exit 1
fi
