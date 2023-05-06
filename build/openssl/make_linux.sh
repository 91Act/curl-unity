#!/usr/bin/env bash

set -exuo pipefail

OPENSSL_VERSION=openssl-1.1.1t
OPENSSL_ROOT="$PWD/$OPENSSL_VERSION"

PREBUILT_DIR="$PWD/prebuilt/linux"

rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"
rm -rf "${OPENSSL_VERSION}" && tar -xf "${OPENSSL_VERSION}.tar.gz"

(
    cd "$OPENSSL_ROOT"
    CFLAGS="-fvisibility=hidden" \
        ./Configure linux-x86_64 \
        --prefix="$PREBUILT_DIR" \
        no-shared no-engine \
        -fvisibility=hidden

    make clean
    make install_dev -j8
)

mkdir -p "$PREBUILT_DIR"
