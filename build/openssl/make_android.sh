#!/usr/bin/env bash

set -exuo pipefail

OPENSSL_VERSION=openssl-1.1.1s
OPENSSL_ROOT="$PWD/$OPENSSL_VERSION"
PREBUILT_DIR_ROOT=$PWD/prebuilt/android

export ANDROID_NDK_HOME="${ANDROID_NDK_HOME:-/usr/local/android-ndk-r16b}"

do_make()
{
    case $1 in
    arm)
        CONF_ARCH=android-arm
        ABI=armeabi-v7a
        TOOLCHAIN=arm-linux-androideabi-4.9
    ;;
    arm64)
        CONF_ARCH=android-arm64
        ABI=arm64-v8a
        TOOLCHAIN=aarch64-linux-android-4.9
    ;;
    x86)
        CONF_ARCH=android-x86
        ABI=x86
        TOOLCHAIN=x86-4.9
    ;;
    *)
    exit
    ;;
    esac

    PREBUILT_DIR="$PREBUILT_DIR_ROOT/$ABI"

    rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"
    rm -rf "${OPENSSL_VERSION}" && tar -xf "${OPENSSL_VERSION}.tar.gz"

    (
        cd "$OPENSSL_ROOT"

        PATH="$ANDROID_NDK_HOME/toolchains/$TOOLCHAIN/prebuilt/darwin-x86_64/bin:$PATH"

        ./Configure $CONF_ARCH --prefix="$PREBUILT_DIR" -D__ANDROID_API__=21 no-asm no-shared no-unit-test

        make clean
        make install_dev -j8
    )
}

do_make arm
do_make arm64
do_make x86
