for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_ios.sh)
done

mkdir -p ../Assets/curl-unity/Plugins/ios

libtool -static -o ../Assets/curl-unity/Plugins/ios/libcurl.a \
    openssl/prebuilt/ios/lib/libssl.a \
    openssl/prebuilt/ios/lib/libcrypto.a \
    nghttp2/prebuilt/ios/lib/libnghttp2.a \
    curl/prebuilt/ios/lib/libcurl.a