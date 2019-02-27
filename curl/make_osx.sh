CURL_VERSION=curl-7.61.0

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    PROJ_ROOT=`pwd`
    BUILD_DIR=$PROJ_ROOT/build/osx
    OPENSSL_ROOT=$PROJ_ROOT/openssl/prebuilt

    export CFLAGS="-I${OPENSSL_ROOT}/osx/include -I${OPENSSL_ROOT}/include"
    export LDFLAGS="-L${OPENSSL_ROOT}/osx/lib"

    cd $CURL_VERSION
    ./configure --host=i686-apple-darwin --prefix=$BUILD_DIR --with-ssl --enable-ipv6 --disable-shared

    make clean
    make -j8
    make install

    cd -
}

do_make