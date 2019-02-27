OPEN_SSL_VERSION=openssl-OpenSSL_1_1_1a

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    BUILD_DIR=`pwd`/build/ios
    PREBUILT_DIR=`pwd`/prebuilt/ios
    export CC='/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang'
    export CROSS_TOP=/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer
    export CROSS_SDK=iPhoneOS.sdk

    (
        cd $OPEN_SSL_VERSION
        ./Configure ios64-cross --prefix=$BUILD_DIR no-shared no-dso no-hw no-engine
        make clean
        make build_libs
        make install_dev -j8
    )
    
    mkdir -p $PREBUILT_DIR
    cp -r $BUILD_DIR/include $PREBUILT_DIR
    cp -r $BUILD_DIR/lib $PREBUILT_DIR
}

do_make