OPEN_SSL_VERSION=openssl-1.1.1b

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    PREBUILT_DIR=`pwd`/prebuilt/ios
    export XCODE=`xcode-select --print-path`
    export CC='${XCODE}/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang -fembed-bitcode'
    export CROSS_TOP=${XCODE}/Platforms/iPhoneOS.platform/Developer
    export CROSS_SDK=iPhoneOS.sdk

    (
        cd $OPEN_SSL_VERSION
        ./Configure ios64-cross --prefix=$PREBUILT_DIR no-shared no-dso no-hw no-engine
        make clean
        make install_dev -j8
    )
    
    mkdir -p $PREBUILT_DIR
}

do_make
