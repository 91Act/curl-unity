PWD=`pwd`
CURL_VERSION=curl-7.64.0
BUILD_DIR_ROOT=$PWD/build/android
PREBUILT_DIR_ROOT=$PWD/prebuilt/android

export ANDROID_NDK_HOME=/usr/local/android-ndk-r16b

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    case $1 in
    arm)
        CONF_ARCH=android-arm
        ABI=armeabi-v7a
        TOOLCHAIN=arm-linux-androideabi
    ;;
    arm64)
        CONF_ARCH=android-arm64
        ABI=arm64-v8a
        TOOLCHAIN=aarch64-linux-android
    ;;
    x86)
        CONF_ARCH=android-x86
        ABI=x86
        TOOLCHAIN=i686-linux-android
    ;;
    *)
    exit
    ;;
    esac

    STANDALONE_TOOLCHAIN=$BUILD_DIR_ROOT/toolchains/$TOOLCHAIN

    if [ ! -d $STANDALONE_TOOLCHAIN ]; then
        python $ANDROID_NDK_HOME/build/tools/make_standalone_toolchain.py --arch $1 --api 27 --install-dir=$STANDALONE_TOOLCHAIN
    fi

    PREBUILT_DIR=$PREBUILT_DIR_ROOT/$ABI
    OPENSSL_ROOT=$PWD/../openssl/prebuilt/android/$ABI
    NGHTTP2_ROOT=$PWD/../nghttp2/prebuilt/android/$ABI

    (
        cd $CURL_VERSION

        export PATH=$STANDALONE_TOOLCHAIN/bin:$PATH
        # export PATH=$ANDROID_NDK_HOME/toolchains/$TOOLCHAIN/prebuilt/darwin-x86_64/bin:$PATH

        ./configure \
            --host=$TOOLCHAIN \
            --target=$TOOLCHAIN \
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

# do_make arm
# do_make arm64
do_make x86