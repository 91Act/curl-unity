#!/usr/bin/env bash

set -exuo pipefail

OPENSSL_VERSION=openssl-1.1.1t

PREBUILT_DIR="$PWD/prebuilt/osx"

rm -rf "${PREBUILT_DIR}" && mkdir -p "${PREBUILT_DIR}"
rm -rf "${OPENSSL_VERSION}" && tar -xf "${OPENSSL_VERSION}.tar.gz"

cd "$OPENSSL_VERSION" || exit 1
./Configure darwin64-x86_64-cc --prefix="$PREBUILT_DIR" no-asm no-shared no-unit-test

make clean
make install_dev -j8
