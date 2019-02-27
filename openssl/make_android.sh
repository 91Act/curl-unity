OPEN_SSL_VERSION=openssl-1.1.0h

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    CONF=$1
    ARCH=$2
    TOOL=$3
    NDK_TOOLCHAIN=$4

    BUILD_DIR=`pwd`/build/android/$ARCH

    mkdir -p $BUILD_DIR

    TOOLCHAIN=$BUILD_DIR/toolchain

    if [ ! -d $TOOLCHAIN ]; then
        $ANDROID_NDK_ROOT/build/tools/make-standalone-toolchain.sh --arch=$ARCH --platform=android-21 --toolchain=$NDK_TOOLCHAIN --install-dir=$TOOLCHAIN
    fi

    export CROSS_SYSROOT=$TOOLCHAIN/sysroot
    export CC=$TOOLCHAIN/bin/${TOOL}-gcc
    export LD=$TOOLCHAIN/bin/${TOOL}-ld
    export AR=$TOOLCHAIN/bin/${TOOL}-ar
    export RANLIB=$TOOLCHAIN/bin/${TOOL}-ranlib    

    cd $OPEN_SSL_VERSION

    ./Configure $CONF -D__ARM_MAX_ARCH__=7 --prefix=$BUILD_DIR no-asm no-shared no-unit-test

    make clean
    make -j8
    make install_sw
    cd -
}

do_make android-armeabi arm arm-linux-androideabi arm-linux-androideabi-4.8
do_make android-x86 x86 i686-linux-android x86-4.8