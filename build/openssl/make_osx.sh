OPEN_SSL_VERSION=openssl-1.1.1b

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    PREBUILT_DIR=`pwd`/prebuilt/osx
    (
        cd $OPEN_SSL_VERSION
        ./Configure darwin64-x86_64-cc --prefix=$PREBUILT_DIR no-asm no-shared no-unit-test

        make clean
        make install_dev -j8
    )
}

do_make

