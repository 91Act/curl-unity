PWD=`pwd`
OPENSSL_VERSION=openssl-1.1.1b
OPENSSL_ROOT=$PWD/$OPENSSL_VERSION
PREBUILT_DIR_ROOT=$PWD/prebuilt/android

export ANDROID_NDK_HOME=/usr/local/android-ndk-r16b

if [ ! -d $OPENSSL_ROOT ]; then    
    tar xzf ${OPENSSL_ROOT}.tar.gz
fi

do_make()
{
    case $1 in
    arm)
        CONF_ARCH=android-arm
        ABI=armeabi-v7a
        TOOLCHAIN=arm-linux-androideabi-4.9
    ;;
    arm64)
        CONF_ARCH=android-arm64
        ABI=arm64-v8a
        TOOLCHAIN=aarch64-linux-android-4.9
    ;;
    x86)
        CONF_ARCH=android-x86
        ABI=x86
        TOOLCHAIN=x86-4.9
    ;;
    *)
    exit
    ;;
    esac

    PREBUILT_DIR=$PREBUILT_DIR_ROOT/$ABI

    PATH=$ANDROID_NDK_HOME/toolchains/$TOOLCHAIN/prebuilt/darwin-x86_64/bin:$PATH

    (
        cd $OPENSSL_ROOT

        ./Configure $CONF_ARCH --prefix=$PREBUILT_DIR -D__ANDROID_API__=27 no-asm no-shared no-unit-test

        make clean
        make install_dev -j8
    )
}

do_make arm
do_make arm64
do_make x86