export NGHTTP2_ROOT=nghttp2-1.36.0
export BUILD_DIR=build/macOS
export PREBUILT_DIR=prebuilt/macOS

mkdir -p $BUILD_DIR && cd $BUILD_DIR
cmake ../../$NGHTTP2_ROOT -DENABLE_LIB_ONLY=ON -DENABLE_STATIC_LIB=ON -DENABLE_SHARED_LIB=OFF
cd -
cmake --build $BUILD_DIR --config Release

mkdir -p $PREBUILT_DIR/include
cp $NGHTTP2_ROOT/lib/includes/nghttp2/* $PREBUILT_DIR/include

mkdir -p $PREBUILT_DIR/lib
cp $BUILD_DIR/lib/libnghttp2.a $PREBUILT_DIR/lib/libnghttp2_static.a
