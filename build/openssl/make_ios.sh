#!/usr/bin/env bash

set -exuo pipefail

OPENSSL_VERSION=openssl-1.1.1t

PREBUILT_DIR="$PWD/prebuilt/linux"

rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"
rm -rf "${OPENSSL_VERSION}" && tar -xf "${OPENSSL_VERSION}.tar.gz"

XCODE="$(xcode-select --print-path)"
export CC="${XCODE}/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang -fembed-bitcode"
export CROSS_TOP=${XCODE}/Platforms/iPhoneOS.platform/Developer
export CROSS_SDK=iPhoneOS.sdk

cd $OPENSSL_VERSION
./Configure ios64-cross --prefix="$PREBUILT_DIR" no-shared no-dso no-hw no-engine
make clean
make install_dev -j8
