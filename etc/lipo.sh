lipo -create \
    -arch x86_64 build/curl/prebuilt/osx/lib/libcurl.dylib \
    -arch arm64e build/curl/prebuilt/osx_arm/lib/libcurl.dylib \
    -output Assets/curl-unity/Plugins/osx/curl.bundle/Contents/MacOS/curl
