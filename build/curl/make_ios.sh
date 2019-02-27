CURL_VERSION=curl-7.64.0

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    PROJ_ROOT=`pwd`
    BUILD_DIR=$PROJ_ROOT/build/ios
    PREBUILT_DIR=$PROJ_ROOT/prebuilt/ios
    OPENSSL_ROOT=$PROJ_ROOT/../openssl/prebuilt/ios
    NGHTTP2_ROOT=$PROJ_ROOT/../nghttp2/prebuilt/ios

    XCODE="/Applications/Xcode.app/Contents/Developer"
    SDK_ROOT="${XCODE}/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS.sdk"
    SDK_VERSION=12.0

    export CC="${XCODE}/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang"
    export CFLAGS="-DCURL_BUILD_IOS -arch arm64 -isysroot ${SDK_ROOT} -miphoneos-version-min=${SDK_VERSION}"
    export PKG_CONFIG_PATH=$NGHTTP2_ROOT:$PKG_CONFIG_PATH
    
    (
        cd $CURL_VERSION
        ./configure \
            --host=arm-apple-darwin \
            --prefix=$BUILD_DIR \
            --with-ssl=$OPENSSL_ROOT \
            --with-nghttp2 \
            --enable-ipv6 \
            --disable-ftp \
            --disable-file \
            --disable-ldap \
            --disable-ldaps \
            --disable-rtsp \
            --disable-proxy \
            --disable-dict \
            --disable-telnet \
            --disable-tftp \
            --disable-pop3 \
            --disable-imap \
            --disable-smb \
            --disable-smtp \
            --disable-gopher \
            --disable-manual \
            --disable-shared

        make clean
        make install -j8
    )

    # mkdir -p $PREBUILT_DIR
    # cp -r $BUILD_DIR/* $PREBUILT_DIR
}

do_make