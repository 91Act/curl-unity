export NGHTTP2_ROOT=nghttp2-1.36.0
export BUILD_DIR=`pwd`/build/ios
export PREBUILT_DIR=`pwd`/prebuilt/ios

mkdir -p $BUILD_DIR 
(
    cd $BUILD_DIR
    cmake ../../$NGHTTP2_ROOT -DCMAKE_TOOLCHAIN_FILE=../../../cmake/ios.toolchain.cmake -DIOS_PLATFORM=OS64 -DENABLE_BITCODE=0 -DENABLE_LIB_ONLY=ON -DENABLE_STATIC_LIB=ON -DENABLE_SHARED_LIB=OFF
)
cmake --build $BUILD_DIR --config Release

mkdir -p $PREBUILT_DIR/include
cp -r $NGHTTP2_ROOT/lib/includes/nghttp2 $PREBUILT_DIR/include

mkdir -p $PREBUILT_DIR/lib
cp $BUILD_DIR/lib/libnghttp2.a $PREBUILT_DIR/lib

cat > $PREBUILT_DIR/libnghttp2.pc << EOF
Name: libnghttp2
Description: HTTP/2 C library
URL: https://github.com/tatsuhiro-t/nghttp2
Version: 1.36.0
Libs: -L$PREBUILT_DIR/lib -lnghttp2
Cflags: -I$PREBUILT_DIR/include
EOF