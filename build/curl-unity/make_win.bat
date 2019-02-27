set PROJ=%CD%
set BUILD_DIR=%PROJ%\build\win
set PREBUILT_DIR=%PROJ%\prebuilt\win

mkdir %BUILD_DIR% & pushd %BUILD_DIR%

cmake %PROJ% ^
    -DCMAKE_INSTALL_PREFIX=%PREBUILT_DIR% ^
    -G"Visual Studio 15 2017 Win64"

popd

cmake --build %BUILD_DIR% --config Release --target install