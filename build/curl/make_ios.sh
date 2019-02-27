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

    XCODE=`xcode-select --print-path`
    SDK_ROOT="${XCODE}/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS.sdk"
    SDK_VERSION=12.0

    export CC="${XCODE}/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang"
    export CFLAGS="-arch arm64 -pipe -Os -gdwarf-2 -isysroot ${SDK_ROOT} -miphoneos-version-min=${SDK_VERSION}"    
    
    (
        cd $CURL_VERSION
        ./configure \
            --host=arm-apple-darwin \
            --prefix=$PREBUILT_DIR \
            --with-ssl=$OPENSSL_ROOT \
            --with-nghttp2=$NGHTTP2_ROOT \
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
}

do_make