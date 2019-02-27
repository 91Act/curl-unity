PWD=`pwd`
CURL_VERSION=curl-7.64.0

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    BUILD_DIR=$PWD/build/osx
    PREBUILT_DIR=$PWD/prebuilt/osx
    OPENSSL_ROOT=$PWD/../openssl/prebuilt/osx
    NGHTTP2_ROOT=$PWD/../nghttp2/prebuilt/osx

    (
        cd $CURL_VERSION
        ./configure \
            --host=i686-apple-darwin \
            --prefix=$BUILD_DIR \
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

    mkdir -p $PREBUILT_DIR
    cp -r $BUILD_DIR/* $PREBUILT_DIR
}

do_make