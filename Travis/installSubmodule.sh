#! /bin/bash

set -e

echo "Installing the base .NET SDK into the sdk..."
git clone --branch=feature-dotNetSDKChanges https://github.com/mediumTaj/csharp-sdk-base.git /home/travis/build/mediumTaj/dot-net-sdk/sdk/src/services
