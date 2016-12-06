if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  echo '$TRAVIS_PULL_REQUEST is false, running tests'
  echo "Attempting to move appdata directory..."
  mv Test/Test/appdata/ Test/Test/bin/Release/appdata/
  if [ $? = 0 ] ; then
    echo "Move appdata directory COMPLETED! Exited with $?"
    echo "Attempting to decrypt config..."
    openssl aes-256-cbc -K $encrypted_89fb597d004e_key -iv $encrypted_89fb597d004e_iv -in Config.json.enc -out Test/Test/bin/Release/appdata/Config.json -d
    if [ $? = 0 ] ; then
      echo "Decrypting config COMPLETED! Exited with $?"
      echo "Attempting to run dot-net-sdk integration Tests..."
      mono ./testrunner/NUnit.Runners.2.6.4/tools/nunit-console.exe Test/Test/bin/Release/Test.dll
      if [ $? = 0 ] ; then
        echo "Integration tests COMPLETED! Exited with $?"
        exit 0
      else
        echo "Integration tests FAILED! Exited with $?"
        exit 1
      fi
    else
      echo "Decrypting config FAILED! Exited with $?"
      exit 1
    fi
  else
    echo "Moving appdata directory FAILED! Exited with $?"
    exit 1
  fi
else
  echo '$TRAVIS_PULL_REQUEST is not false ($TRAVIS_PULL_REQUEST), skipping tests'
fi
