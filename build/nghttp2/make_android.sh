PWD=`pwd`
NGHTTP2_ROOT=$PWD/nghttp2-1.36.0
BUILD_DIR_ROOT=$PWD/build/android
PREBUILT_DIR_ROOT=$PWD/prebuilt/android

export ANROID_NDK=/usr/local/android-ndk-r16b

function domake()
{
    arch=$1
    toolchain=$2

    BUILD_DIR=$BUILD_DIR_ROOT/${arch}
    PREBUILT_DIR=$PREBUILT_DIR_ROOT/${arch}

    mkdir -p $BUILD_DIR 
    (
        cd $BUILD_DIR
        cmake $NGHTTP2_ROOT \
            -DCMAKE_TOOLCHAIN_FILE=${ANROID_NDK}/build/cmake/android.toolchain.cmake \
            -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
            -DANDROID_ABI=${arch} \
            -DANDROID_TOOLCHAIN_NAME=${toolchain} \
            -DANDROID_NATIVE_API_LEVEL=android-14 \
            -DENABLE_LIB_ONLY=ON \
            -DENABLE_STATIC_LIB=ON \
            -DENABLE_SHARED_LIB=OFF
    )
    cmake --build $BUILD_DIR --config Release --target install
}

domake armeabi-v7a arm-linux-androideabi-4.9
domake arm64-v8a arm-linux-androideabi-4.9
domake x86 x86-4.9