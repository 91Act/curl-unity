CURL_VERSION=curl-7.61.0

if [ ! -d $CURL_VERSION ]; then    
    tar xzf ${CURL_VERSION}.tar.gz
fi

do_make()
{
    PROJ_ROOT=`pwd`
    BUILD_DIR=$PROJ_ROOT/build/ios
    OPENSSL_ROOT=$PROJ_ROOT/openssl/prebuilt

    mkdir -p $BUILD_DIR

    export CC=/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/clang
    export SYSROOT=/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS.sdk

    export CFLAGS="-arch arm64 -isysroot ${SYSROOT} -miphoneos-version-min=8.0 -I${OPENSSL_ROOT}/ios/include -I${OPENSSL_ROOT}/include"
    export LDFLAGS="-arch arm64 -isysroot ${SYSROOT} -L${OPENSSL_ROOT}/ios/lib"

    cd $CURL_VERSION
    ./configure --host=arm-apple-darwin --prefix=$BUILD_DIR --with-ssl --enable-ipv6 --disable-shared
    make -j8
    make install
    cd -
}

do_make