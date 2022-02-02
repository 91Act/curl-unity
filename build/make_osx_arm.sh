#!/usr/bin/env bash

set -exuo pipefail

for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_osx_arm.sh)
done

mkdir -p ../Assets/curl-unity/Plugins/osx_arm/curl.bundle/Contents/MacOS/
cp curl/prebuilt/osx_arm/lib/libcurl.dylib ../Assets/curl-unity/Plugins/osx_arm/curl.bundle/Contents/MacOS/curl
