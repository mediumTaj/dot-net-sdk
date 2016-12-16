#! /bin/bash

set -e

echo "Restoring packages!"
nuget restore Test/Test.sln

echo "Building sdk!"
xbuild /p:Configuration=Release sdk/sdk.sln

echo "Building Tests"
xbuild /p:Configuration=Release Test/Test.sln
