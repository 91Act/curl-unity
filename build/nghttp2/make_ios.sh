export NGHTTP2_ROOT=nghttp2-1.36.0
export BUILD_DIR=`pwd`/build/ios
export PREBUILT_DIR=`pwd`/prebuilt/ios

mkdir -p $BUILD_DIR 
(
    cd $BUILD_DIR
    cmake ../../$NGHTTP2_ROOT \
        -DCMAKE_TOOLCHAIN_FILE=../../../cmake/ios.toolchain.cmake \
        -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
        -DIOS_PLATFORM=OS64 \
        -DENABLE_BITCODE=0 \
        -DENABLE_LIB_ONLY=ON \
        -DENABLE_STATIC_LIB=ON \
        -DENABLE_SHARED_LIB=OFF
)
cmake --build $BUILD_DIR --config Release --target install