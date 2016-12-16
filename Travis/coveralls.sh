#!/bin/bash

set -e

if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  echo '$TRAVIS_PULL_REQUEST is false, sending coverage'

  if [ -n "$COVERALLS_REPO_TOKEN" ]
  then
    nuget install -OutputDirectory Test/packages -Version 0.7.0 coveralls.net
    COVERALLS=Test/packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe
    echo $COVERALLS_REPO_TOKEN
    chmod +x $COVERALLS
    $COVERALLS --opencover -i reports/results.xml --repoToken $COVERALLS_REPO_TOKEN --serviceName "travis-ci" --useRelativePaths
  else
    echo 'No coveralls repo token found!'
  fi

else
  echo '$TRAVIS_PULL_REQUEST is not false ($TRAVIS_PULL_REQUEST), not sending coverage'
fi
