OPEN_SSL_VERSION=openssl-1.1.0h

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    BUILD_DIR=`pwd`/build/osx

    cd $OPEN_SSL_VERSION
    ./Configure darwin64-x86_64-cc --prefix=$BUILD_DIR no-asm no-shared no-unit-test

    sed -i '' 's/AR=$(CROSS_COMPILE)ar $(ARFLAGS) r/AR=$(CROSS_COMPILE)libtool -o $(ARFLAGS)/g' Makefile

    make clean
    make -j8
    make install_sw

    cd -
}

do_make