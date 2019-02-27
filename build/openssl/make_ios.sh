OPEN_SSL_VERSION=openssl-1.1.0h

if [ ! -d $OPEN_SSL_VERSION ]; then    
    tar xzf ${OPEN_SSL_VERSION}.tar.gz
fi

do_make()
{
    BUILD_DIR=`pwd`/build/ios
    export CC='/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang -arch arm64'
    export CROSS_TOP=/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer
    export CROSS_SDK=iPhoneOS.sdk

    cd $OPEN_SSL_VERSION
    ./Configure iphoneos-cross --prefix=$BUILD_DIR no-asm no-shared no-unit-test
    make -j8
    make install_sw
    cd -
}

do_make