#!/usr/bin/env bash

set -exuo pipefail

CURL_VERSION=curl-7.87.0
CURL_ROOT="$PWD/$CURL_VERSION"

do_make()
{
    BUILD_DIR="$PWD/build/osx_arm"
    PREBUILT_DIR="$PWD/prebuilt/osx_arm"
    OPENSSL_ROOT="$PWD/../openssl/prebuilt/osx_arm"
    NGHTTP2_ROOT="$PWD/../nghttp2/prebuilt/osx_arm"

    rm -rf "${BUILD_DIR}" && mkdir -p "${BUILD_DIR}"
    rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"

    (
        cd "$BUILD_DIR" && \
        cmake "$CURL_ROOT" \
            -DCMAKE_INSTALL_PREFIX="$PREBUILT_DIR" \
            -DOPENSSL_ROOT_DIR="$OPENSSL_ROOT" \
            -DOPENSSL_INCLUDE_DIR="$OPENSSL_ROOT/include" \
            -DOPENSSL_CRYPTO_LIBRARY="$OPENSSL_ROOT/lib/libcrypto.a" \
            -DOPENSSL_SSL_LIBRARY="$OPENSSL_ROOT/lib/libssl.a" \
            -DNGHTTP2_INCLUDE_DIR="$NGHTTP2_ROOT/include" \
            -DNGHTTP2_LIBRARY="$NGHTTP2_ROOT/lib/libnghttp2.a" \
            -DUSE_NGHTTP2=ON \
            -DHTTP_ONLY=ON \
            -DCMAKE_USE_LIBSSH2=OFF \
            -DBUILD_TESTING=OFF \
            -DBUILD_SHARED_LIBS=ON \
            -DCMAKE_OSX_ARCHITECTURES=arm64
    )
    cmake --build "$BUILD_DIR" --config Release --target install -j8
}

do_make