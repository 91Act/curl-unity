PROJ=`pwd`
CURL_VERSION=curl-7.64.0
CURL_ROOT=$PROJ/$CURL_VERSION

do_make()
{
    BUILD_DIR=$PROJ/build/osx
    PREBUILT_DIR=$PROJ/prebuilt/osx
    OPENSSL_ROOT=$PROJ/../openssl/prebuilt/osx
    NGHTTP2_ROOT=$PROJ/../nghttp2/prebuilt/osx

    mkdir -p $BUILD_DIR
    (
        cd $BUILD_DIR
        cmake $CURL_ROOT \
            -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
            -DOPENSSL_ROOT_DIR=$OPENSSL_ROOT \
            -DOPENSSL_INCLUDE_DIR=$OPENSSL_ROOT/include \
            -DOPENSSL_CRYPTO_LIBRARY=$OPENSSL_ROOT/lib/libcrypto.a \
            -DOPENSSL_SSL_LIBRARY=$OPENSSL_ROOT/lib/libssl.a \
            -DNGHTTP2_INCLUDE_DIR=$NGHTTP2_ROOT/include \
            -DNGHTTP2_LIBRARY=$NGHTTP2_ROOT/lib/libnghttp2.a \
            -DUSE_NGHTTP2=ON \
            -DHTTP_ONLY=ON \
            -DCMAKE_USE_LIBSSH2=OFF \
            -DBUILD_TESTING=OFF \
            -DBUILD_SHARED_LIBS=ON
    )
    cmake --build $BUILD_DIR --config Release --target install -j8
}

do_make