#!/usr/bin/env bash

set -exuo pipefail

NGHTTP2_ROOT="$PWD/nghttp2-1.51.0"
BUILD_DIR="$PWD/build/linux"
PREBUILT_DIR="$PWD/prebuilt/linux"

rm -rf "${BUILD_DIR}" && mkdir -p "${BUILD_DIR}"
rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"

mkdir -p "$BUILD_DIR"
(
    cd "$BUILD_DIR" && \
    cmake "$NGHTTP2_ROOT" \
        -DCMAKE_INSTALL_PREFIX="$PREBUILT_DIR" \
        -DCMAKE_POSITION_INDEPENDENT_CODE=ON \
        -DENABLE_LIB_ONLY=ON \
        -DENABLE_STATIC_LIB=ON \
        -DENABLE_SHARED_LIB=OFF
)
cmake --build "$BUILD_DIR" --config Release --target install
