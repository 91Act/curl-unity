#!/usr/bin/env bash

set -exuo pipefail

for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_linux.sh)
done

mkdir -p ../Assets/curl-unity/Plugins/linux
cp -f curl/prebuilt/linux/lib64/libcurl.so ../Assets/curl-unity/Plugins/linux
