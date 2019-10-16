PWD=`pwd`
NGHTTP2_ROOT=$PWD/nghttp2-1.36.0
BUILD_DIR_ROOT=$PWD/build/android
PREBUILT_DIR_ROOT=$PWD/prebuilt/android

export ANROID_NDK=/usr/local/android-ndk-r16b

function do_make()
{
    ARCH=$1
    TOOLCHAIN=$2

    BUILD_DIR=$BUILD_DIR_ROOT/$ARCH
    PREBUILT_DIR=$PREBUILT_DIR_ROOT/$ARCH

    mkdir -p $BUILD_DIR 
    (
        cd $BUILD_DIR
        cmake $NGHTTP2_ROOT \
            -DCMAKE_TOOLCHAIN_FILE=$ANROID_NDK/build/cmake/android.TOOLCHAIN.cmake \
            -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
            -DANDROID_ABI=$ARCH \
            -DANDROID_TOOLCHAIN_NAME=$TOOLCHAIN \
            -DANDROID_NATIVE_API_LEVEL=android-21 \
            -DENABLE_LIB_ONLY=ON \
            -DENABLE_STATIC_LIB=ON \
            -DENABLE_SHARED_LIB=OFF
    )
    cmake --build $BUILD_DIR --config Release --target install
}

do_make armeabi-v7a arm-linux-androideabi-4.9
do_make arm64-v8a aARCH64-linux-android-4.9
do_make x86 x86-4.9
