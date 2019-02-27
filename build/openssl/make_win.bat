set BUILD_DIR=%CD%\build\x86_64
set PREBUILT_DIR=%CD%\prebuilt\x86_64
set OPENSSL_ROOT=openssl-OpenSSL_1_1_1a

mkdir %BUILD_DIR%

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"

pushd %OPENSSL_ROOT%
perl Configure VC-WIN64A --prefix=%BUILD_DIR% no-asm no-shared no-unit-test no-external-tests
set CL=/MP
nmake build_libs_nodep
nmake install_dev
popd

mkdir %PREBUILT_DIR%
xcopy %BUILD_DIR%\include %PREBUILT_DIR%\inculde /e /s /y/ i
xcopy %BUILD_DIR%\lib %PREBUILT_DIR%\lib /e /s /y/ i