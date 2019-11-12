set PROJ=%CD%
set CURL_ROOT=%PROJ%\curl-7.67.0
set BUILD_DIR=%PROJ%\build\win
set PREBUILT_DIR=%PROJ%\prebuilt\win
set NGHTTP2_ROOT=%PROJ%\..\nghttp2\prebuilt\win
set OPENSSL_ROOT=%PROJ%\..\openssl\prebuilt\win

mkdir %BUILD_DIR%
pushd %BUILD_DIR%

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"

cmake %CURL_ROOT% ^
    -A x64 ^
    -DCMAKE_INSTALL_PREFIX=%PREBUILT_DIR% ^
    -DCMAKE_USE_OPENSSL=ON ^
    -DOPENSSL_ROOT_DIR=%OPENSSL_ROOT% ^
    -DOPENSSL_INCLUDE_DIR=%OPENSSL_ROOT%\include ^
    -DOPENSSL_CRYPTO_LIBRARY=%OPENSSL_ROOT%\lib\libcrypto.lib ^
    -DOPENSSL_SSL_LIBRARY=%OPENSSL_ROOT%\lib\libssl.lib ^
    -DUSE_NGHTTP2=ON ^
    -DNGHTTP2_INCLUDE_DIR=%NGHTTP2_ROOT%\include ^
    -DNGHTTP2_LIBRARY=%NGHTTP2_ROOT%\lib\nghttp2.lib ^
    -DHTTP_ONLY=ON ^
    -DCMAKE_USE_LIBSSH2=OFF ^
    -DBUILD_TESTING=OFF ^
    -DBUILD_SHARED_LIBS=ON
popd

cmake --build %BUILD_DIR% --config Release --target install