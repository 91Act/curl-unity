set BUILD_DIR=%CD%\build\x86_64
set PREBUILT_DIR=%CD%\prebuilt\x86_64
set OPENSSL_ROOT=openssl-1.1.1b

mkdir %BUILD_DIR%

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"

pushd %OPENSSL_ROOT%
perl Configure VC-WIN64A --prefix=%PREBUILT_DIR% no-asm no-shared no-unit-test
set CL=/MP
nmake install_dev
popd