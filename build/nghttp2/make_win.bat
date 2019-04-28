set NGHTTP2_ROOT=%CD%\nghttp2-1.36.0
set BUILD_DIR=%CD%\build\win
set PREBUILT_DIR=%CD%\prebuilt\win

mkdir %BUILD_DIR% & pushd %BUILD_DIR%

cmake %NGHTTP2_ROOT% ^
    -A x64 ^
    -DCMAKE_INSTALL_PREFIX=%PREBUILT_DIR% ^
    -DENABLE_LIB_ONLY=ON ^
    -DENABLE_STATIC_LIB=ON ^
    -DENABLE_SHARED_LIB=OFF

popd

cmake --build %BUILD_DIR% --config Debug --target install