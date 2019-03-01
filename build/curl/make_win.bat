set PROJ_ROOT=%CD%
set PREBUILT_DIR=%PROJ_ROOT%\prebuilt\win
set CURL_ROOT=curl-7.64.0
set NGHTTP2_ROOT=%PROJ_ROOT%\..\nghttp2\prebuilt\win
set SSL_ROOT=%PROJ_ROOT%\..\openssl\prebuilt\win

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"

pushd %CURL_ROOT%\winbuild
nmake /f Makefile.vc mode=dll VC=15 MACHINE=x64 ^
    WITH_NGHTTP2=static NGHTTP2_PATH=%NGHTTP2_ROOT% ^
    WITH_SSL=static SSL_PATH=%SSL_ROOT% ^
    ENABLE_IDN=no ^
    ENABLE_IPV6=yes 
popd

mkdir %PREBUILT_DIR%
set BUILD_DIR=%PROJ_ROOT%\%CURL_ROOT%\builds\libcurl-vc15-x64-release-dll-ssl-static-ipv6-sspi-nghttp2-static
xcopy %BUILD_DIR%\include %PREBUILT_DIR%\include\ /s /e /y /i
xcopy %BUILD_DIR%\lib %PREBUILT_DIR%\lib\ /s /e /y /i
xcopy %BUILD_DIR%\bin\*.dll %PREBUILT_DIR%\lib\ /s /e /y /i
xcopy %BUILD_DIR%\bin\*.exe %PREBUILT_DIR%\bin\ /s /e /y /i