#!/usr/bin/env bash

set -exuo pipefail

CURL_VERSION=curl-7.87.0
CURL_ROOT="$PWD/$CURL_VERSION"
BUILD_DIR_ROOT="$PWD/build/android"
PREBUILT_DIR_ROOT="$PWD/prebuilt/android"

export ANDROID_NDK_HOME="${ANDROID_NDK_HOME:-/usr/local/android-ndk-r16b}"
TOOLCHAIN="${ANDROID_NDK_HOME}/build/cmake/android.TOOLCHAIN.cmake"

do_make()
{
    case $1 in
    arm)
        ABI=armeabi-v7a
        NDK_TOOLCHAIN=arm-linux-androideabi
    ;;
    arm64)
        ABI=arm64-v8a
        NDK_TOOLCHAIN=aarch64-linux-android
    ;;
    x86)
        ABI=x86
        NDK_TOOLCHAIN=i686-linux-android
    ;;
    *)
    exit
    ;;
    esac

    BUILD_DIR="$BUILD_DIR_ROOT/$ABI"
    PREBUILT_DIR="$PREBUILT_DIR_ROOT/$ABI"
    OPENSSL_ROOT="$PWD/../openssl/prebuilt/android/$ABI"
    NGHTTP2_ROOT="$PWD/../nghttp2/prebuilt/android/$ABI"

    rm -rf "${BUILD_DIR}" && mkdir -p "${BUILD_DIR}"
    rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"

    (
        cd "$BUILD_DIR" && \
        cmake "$CURL_ROOT" \
            -DCMAKE_TOOLCHAIN_FILE="$TOOLCHAIN" \
            -DCMAKE_SHARED_LINKER_FLAGS='-s' \
            -DANDROID_ABI=$ABI \
            -DANDROID_TOOLCHAIN_NAME=$NDK_TOOLCHAIN \
            -DANDROID_NATIVE_API_LEVEL=android-21 \
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
            -DENABLE_UNIX_SOCKETS=OFF \
            -DBUILD_TESTING=OFF \
            -DBUILD_SHARED_LIBS=ON \
            -DBUILD_CURL_EXE=OFF
    )
    cmake --build "$BUILD_DIR" --config Release --target install -j8
}

do_make arm
do_make arm64
do_make x86
