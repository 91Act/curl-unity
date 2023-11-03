#!/usr/bin/env bash

set -exuo pipefail

for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_osx.sh)
    (cd $lib && ./make_osx_arm.sh)
done

mkdir -p ../Assets/curl-unity/Plugins/osx/curl.bundle/Contents/MacOS/

lipo -create \
    -arch x86_64 curl/prebuilt/osx/lib/libcurl.dylib \
    -arch arm64e curl/prebuilt/osx_arm/lib/libcurl.dylib \
    -output ../Assets/curl-unity/Plugins/osx/curl.bundle/Contents/MacOS/curl
