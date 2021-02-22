PWD=`pwd`
OPENSSL_VERSION=openssl-1.1.1b
OPENSSL_ROOT=$PWD/$OPENSSL_VERSION

if [ ! -d $OPENSSL_ROOT ]; then    
    tar xzf ${OPENSSL_ROOT}.tar.gz
fi

do_make()
{
    PREBUILT_DIR=`pwd`/prebuilt/linux

    (
        cd $OPENSSL_VERSION
        CFLAGS="-fvisibility=hidden" \
        ./Configure linux-x86_64 \
            --prefix=$PREBUILT_DIR \
            no-shared no-engine \
            -fvisibility=hidden

        make clean
        make install_dev -j8
    )
    
    mkdir -p $PREBUILT_DIR
}

do_make
