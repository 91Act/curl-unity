set PROJ=%CD%
set CURL_ROOT=%PROJ%\curl-7.87.0
set BUILD_DIR=%PROJ%\build\win
set PREBUILT_DIR=%PROJ%\prebuilt\win
set NGHTTP2_ROOT=%PROJ%\..\nghttp2\prebuilt\win
set OPENSSL_ROOT=%PROJ%\..\openssl\prebuilt\win

rd /s /q %BUILD_DIR%
rd /s /q %PREBUILT_DIR%

mkdir %PREBUILT_DIR%
mkdir %BUILD_DIR% & pushd %BUILD_DIR%

call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat"

cmake %CURL_ROOT% ^
    -A x64 ^
    -DCMAKE_INSTALL_PREFIX=%PREBUILT_DIR% ^
    -DCURL_USE_OPENSSL=ON ^
    -DCMAKE_USE_WINSSL=OFF ^
    -DCURL_WINDOWS_SSPI=OFF ^
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
    -DBUILD_SHARED_LIBS=ON ^
    -DCMAKE_C_FLAGS="-DNGHTTP2_STATICLIB"
popd

cmake --build %BUILD_DIR% --config Release --target install
