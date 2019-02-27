OPEN_SSL_VERSION=openssl-OpenSSL_1_1_1a

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    BUILD_DIR=`pwd`/build/osx
    PREBUILT_DIR=`pwd`/prebuilt/osx
    (
        cd $OPEN_SSL_VERSION
        ./Configure darwin64-x86_64-cc --prefix=$BUILD_DIR no-asm no-shared no-unit-test

        make clean
        make install_dev -j8
    )

    mkdir -p $PREBUILT_DIR
    cp -r $BUILD_DIR/include $PREBUILT_DIR
    cp -r $BUILD_DIR/lib $PREBUILT_DIR
}

do_make