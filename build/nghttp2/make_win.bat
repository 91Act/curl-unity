set NGHTTP2_ROOT=nghttp2-1.36.0
set BUILD_DIR=build\x86_64
set PREBUILT_DIR=prebuilt\x86_64

mkdir %BUILD_DIR% & pushd %BUILD_DIR%

cmake ..\..\%NGHTTP2_ROOT% -A x64 -DENABLE_LIB_ONLY=ON -DENABLE_STATIC_LIB=ON -DENABLE_SHARED_LIB=OFF

popd

cmake --build %BUILD_DIR% --config Release

mkdir %PREBUILT_DIR%\include\nghttp2
copy %NGHTTP2_ROOT%\lib\includes\nghttp2\* %PREBUILT_DIR%\include\nghttp2

mkdir %PREBUILT_DIR%\lib
copy %BUILD_DIR%\lib\Release\nghttp2.lib %PREBUILT_DIR%\lib\nghttp2_static.lib