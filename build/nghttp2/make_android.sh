#!/usr/bin/env bash

set -exuo pipefail

NGHTTP2_ROOT="$PWD/nghttp2-1.51.0"
BUILD_DIR_ROOT="$PWD/build/android"
PREBUILT_DIR_ROOT="$PWD/prebuilt/android"

export ANDROID_NDK_HOME="${ANDROID_NDK_HOME:-/usr/local/android-ndk-r16b}"
TOOLCHAIN="${ANDROID_NDK_HOME}/build/cmake/android.TOOLCHAIN.cmake"

function do_make()
{
    case $1 in
    arm)
        ABI=armeabi-v7a
        NDK_TOOLCHAIN=arm-linux-androideabi
    ;;
    arm64)
        ABI=arm64-v8a
        NDK_TOOLCHAIN=aABI64-linux-android
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

    rm -rf "${BUILD_DIR}" && mkdir -p "${BUILD_DIR}"
    rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"

    mkdir -p "$BUILD_DIR"
    (
        cd "$BUILD_DIR" && \
        cmake "$NGHTTP2_ROOT" \
            -DCMAKE_TOOLCHAIN_FILE="$TOOLCHAIN" \
            -DCMAKE_INSTALL_PREFIX="$PREBUILT_DIR" \
            -DANDROID_ABI=$ABI \
            -DANDROID_TOOLCHAIN_NAME=$NDK_TOOLCHAIN \
            -DANDROID_NATIVE_API_LEVEL=android-21 \
            -DENABLE_LIB_ONLY=ON \
            -DENABLE_STATIC_LIB=ON \
            -DENABLE_SHARED_LIB=OFF
    )
    cmake --build "$BUILD_DIR" --config Release --target install
}

do_make arm
do_make arm64
do_make x86
