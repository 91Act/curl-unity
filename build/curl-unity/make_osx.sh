PROJ=`pwd`
BUILD_DIR_ROOT=$PROJ/build/osx
PREBUILT_DIR_ROOT=$PROJ/prebuilt/osx

function do_make()
{
    PREBUILT_DIR=$PREBUILT_DIR_ROOT
    (
        mkdir -p $BUILD_DIR_ROOT && cd $BUILD_DIR_ROOT
        cmake $PROJ \
            -DCMAKE_INSTALL_PREFIX=$PREBUILT_DIR \
            -GXcode
    )
    cmake --build $BUILD_DIR_ROOT --config Release --target install
}

do_make