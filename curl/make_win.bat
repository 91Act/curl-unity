set PROJ_ROOT=%CD%
set BUILD_DIR=%PROJ_ROOT%\build\x86_64
set CURL_ROOT=curl-7.64.0
set NGHTTP2_ROOT=%PROJ_ROOT%\..\nghttp2\prebuilt\x86_64

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"

pushd %CURL_ROOT%\winbuild
set CL=/MP
nmake /f Makefile.vc mode=static VC=15 ENABLE_IPV6=yes MACHINE=x64 WITH_NGHTTP2=static NGHTTP2_PATH=%NGHTTP2_ROOT% WITH_SSL=static SSL_PATH=%PROJ_ROOT%\..\openssl\build\x86_64
popd

mkdir %BUILD_DIR%
xcopy %PROJ_ROOT%\%CURL_ROOT%\builds\libcurl-vc15-x64-release-static-ssl-static-ipv6-sspi-nghttp2-static %BUILD_DIR% /s /e /y