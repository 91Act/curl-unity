#!/usr/bin/env bash

set -exuo pipefail

for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_osx.sh)
done

mkdir -p ../Assets/curl-unity/Plugins/osx/curl.bundle/Contents/MacOS/
cp curl/prebuilt/osx/lib/libcurl.dylib ../Assets/curl-unity/Plugins/osx/curl.bundle/Contents/MacOS/curl
