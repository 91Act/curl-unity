for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_ios.sh)
done

cp openssl/prebuilt/ios/lib/libssl.a ../Assets/curl-unity/Plugins/ios
cp openssl/prebuilt/ios/lib/libcrypto.a ../Assets/curl-unity/Plugins/ios
cp nghttp2/prebuilt/ios/lib/libnghttp2.a ../Assets/curl-unity/Plugins/ios
cp curl/prebuilt/ios/lib/libcurl.a ../Assets/curl-unity/Plugins/ios