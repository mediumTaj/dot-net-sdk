#!/bin/bash

set -e

if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  echo '$TRAVIS_PULL_REQUEST is false, sending coverage'

  if [ -n "$COVERALLS_REPO_TOKEN" ]
  then
    nuget install -OutputDirectory packages -Version 0.7.0 coveralls.net
    Test/packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe --opencover -i reports/results.xml --serviceName "travis-ci" --useRelativePaths
  fi

else
  echo '$TRAVIS_PULL_REQUEST is not false ($TRAVIS_PULL_REQUEST), not sending coverage'
fi
