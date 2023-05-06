set BUILD_DIR=%CD%\build\win
set PREBUILT_DIR=%CD%\prebuilt\win
set OPENSSL_ROOT=openssl-1.1.1s

rd /s /q %BUILD_DIR%
rd /s /q %PREBUILT_DIR%

mkdir %PREBUILT_DIR%
mkdir %BUILD_DIR%

call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat"

tar -xf %OPENSSL_ROOT%.tar.gz

pushd %OPENSSL_ROOT%
perl Configure VC-WIN64A --release --prefix=%PREBUILT_DIR% no-asm no-shared no-unit-test
set CL=/MP
nmake install_dev
popd
