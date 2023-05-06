#!/usr/bin/env bash

set -exuo pipefail

NGHTTP2_ROOT="$PWD/nghttp2-1.52.0"
BUILD_DIR="$PWD/build/ios"
PREBUILT_DIR="$PWD/prebuilt/ios"
TOOLCHAIN="$PWD/../cmake/ios.toolchain.cmake"

rm -rf "${BUILD_DIR}" && mkdir -p "${BUILD_DIR}"
rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"

mkdir -p "$BUILD_DIR"
(
    cd "$BUILD_DIR" && \
    cmake "$NGHTTP2_ROOT" \
        -DCMAKE_TOOLCHAIN_FILE="$TOOLCHAIN" \
        -DCMAKE_INSTALL_PREFIX="$PREBUILT_DIR" \
        -DIOS_PLATFORM=OS64 \
        -DDEPLOYMENT_TARGET=12.0 \
        -DENABLE_BITCODE=1 \
        -DENABLE_LIB_ONLY=ON \
        -DENABLE_STATIC_LIB=ON \
        -DENABLE_SHARED_LIB=OFF
)
cmake --build "$BUILD_DIR" --config Release --target install
