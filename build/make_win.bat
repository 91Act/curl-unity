for %%l in (openssl nghttp2 curl) do (
    pushd %%l
    call make_win.bat
    popd
    )
    
mkdir ..\Assets\curl-unity\Plugins\win
copy curl\prebuilt\win\bin\libcurl.dll ..\Assets\curl-unity\Plugins\win\curl.dll
