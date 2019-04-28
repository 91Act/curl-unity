for %%l in (openssl nghttp2 curl) do (
    pushd %%l
    call make_win.bat
    popd
    )
    
mkdir ..\Assets\curl-unity\Plugins\win
cp curl\prebuilt\win\lib\libcurl_debug.dll ..\Assets\curl-unity\Plugins\win\curl.dll