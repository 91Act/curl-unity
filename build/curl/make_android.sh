CURL_VERSION=curl-7.61.0

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    HOST=$1
    ARCH=$2
    NDK_TOOLCHAIN=$3
    OPENSSL_ARCH=$4

    PROJ_ROOT=`pwd`
    BUILD_DIR=$PROJ_ROOT/build/android/$ARCH
    OPENSSL_ROOT=$PROJ_ROOT/openssl/prebuilt
    TOOLCHAIN=$BUILD_DIR/toolchain

    mkdir -p $BUILD_DIR    

    if [ ! -d $TOOLCHAIN ]; then
        $ANDROID_NDK_ROOT/build/tools/make-standalone-toolchain.sh --arch=$ARCH --platform=android-21 --toolchain=$NDK_TOOLCHAIN --install-dir=$TOOLCHAIN
    fi

    export CROSS_SYSROOT=$TOOLCHAIN/sysroot
    export CC=$TOOLCHAIN/bin/${HOST}-gcc
    export LD=$TOOLCHAIN/bin/${HOST}-ld
    export AR=$TOOLCHAIN/bin/${HOST}-ar
    export RANLIB=$TOOLCHAIN/bin/${HOST}-ranlib    
    export CPPFLAGS="-I${OPENSSL_ROOT}/android/$OPENSSL_ARCH/include -I${OPENSSL_ROOT}/include"
    export LDFLAGS="-L${OPENSSL_ROOT}/android/$OPENSSL_ARCH/lib"

    cd $CURL_VERSION

    ./configure --host=$HOST --prefix=$BUILD_DIR --with-ssl --enable-ipv6 --disable-shared

    make clean
    make -j8
    make install
    cd -
}

do_make arm-linux-androideabi arm arm-linux-androideabi-4.8 armeabi-v7a
do_make i686-linux-android x86 x86-4.8 x86