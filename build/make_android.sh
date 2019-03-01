for lib in openssl nghttp2 curl; do
    (cd $lib && ./make_android.sh)
done

cp curl/prebuilt/android/armeabi-v7a/lib/libcurl.so ../Assets/curl-unity/Plugins/android/armeabi-v7a
cp curl/prebuilt/android/arm64-v8a/lib/libcurl.so ../Assets/curl-unity/Plugins/android/arm64-v8a
cp curl/prebuilt/android/x86/lib/libcurl.so ../Assets/curl-unity/Plugins/android/x86