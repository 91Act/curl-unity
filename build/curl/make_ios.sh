PROJ=`pwd`
CURL_VERSION=curl-7.67.0
CURL_ROOT=$PROJ/$CURL_VERSION
TOOLCHAIN=$PROJ/../cmake/ios.toolchain.cmake

do_make()
{
    BUILD_DIR=$PROJ/build/ios
    PREBUILT_DIR=$PROJ/prebuilt/ios
    OPENSSL_ROOT=$PROJ/../openssl/prebuilt/ios
    NGHTTP2_ROOT=$PROJ/../nghttp2/prebuilt/ios

    mkdir -p $BUILD_DIR
    (
        cd $BUILD_DIR
        cmake $CURL_ROOT \
            -DCMAKE_TOOLCHAIN_FILE=$TOOLCHAIN \
            -DIOS_PLATFORM=OS64 \
            -DDEPLOYMENT_TARGET=12.0 \
            -DENABLE_BITCODE=1 \
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
            -DBUILD_SHARED_LIBS=OFF \
            -DBUILD_CURL_EXE=OFF
    )
    cmake --build $BUILD_DIR --config Release --target install -j8
}

do_make
