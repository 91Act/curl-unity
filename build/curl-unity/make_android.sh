PROJ=`pwd`
BUILD_DIR_ROOT=$PROJ/build/android
PREBUILT_DIR_ROOT=$PROJ/prebuilt/android
ANDROID_NDK=/usr/local/android-ndk-r16b

function do_make()
{
    ABI=$1
    TOOLCHAIN=$2
    PREBUILT_DIR=$PREBUILT_DIR_ROOT/$ABI
    (
        mkdir -p $BUILD_DIR_ROOT/$ABI && cd $BUILD_DIR_ROOT/$ABI
        cmake $PROJ \
            -DANDROID_ABI=$ABI \
            -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
            -DCMAKE_TOOLCHAIN_FILE=$ANDROID_NDK/build/cmake/android.toolchain.cmake \
            -DANDROID_TOOLCHAIN_NAME=${TOOLCHAIN}-4.9 \
            -DANDROID_NATIVE_API_LEVEL=android-27
    )
    cmake --build $BUILD_DIR_ROOT/$ABI --config Release --target install
}

do_make armeabi-v7a arm-linux-androideabi
do_make arm64-v8a aarch64-linux-androideabi
do_make x86 x86