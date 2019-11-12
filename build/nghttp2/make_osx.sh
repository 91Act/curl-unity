PWD=`pwd`
NGHTTP2_ROOT=$PWD/nghttp2-1.39.2
BUILD_DIR=$PWD/build/osx
PREBUILT_DIR=$PWD/prebuilt/osx

mkdir -p $BUILD_DIR 
(
    cd $BUILD_DIR
    cmake $NGHTTP2_ROOT \
        -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
        -DENABLE_LIB_ONLY=ON \
        -DENABLE_STATIC_LIB=ON \
        -DENABLE_SHARED_LIB=OFF
)
cmake --build $BUILD_DIR --config Release --target install