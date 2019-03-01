for %%l in (openssl nghttp2 curl) do (
    pushd %%l
    make_win.bat
    popd
    )

mkdir ..\Assets\curl-unity\Plugins\win
cp build\curl\prebuilt\win\lib\libcurl.dll ..\Assets\curl-unity\Plugins\win\curl.dll