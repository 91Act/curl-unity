# curl-unity

This is a C# wrapper for Unity to use [libcurl](https://github.com/curl/curl) with [http2](https://github.com/curl/curl)/[openssl](https://github.com/openssl/openssl) enabled.

## Supported platforms

* Windows x64
* macOS
* iOS arm64/arm64e
* Android armv7/arm64/x86

## Library versions

|Name|Version|
|-|-|
|curl|7.64.0|
|nghttp2|1.36.0|
|openssl|1.1.1b|

# Build

## Requirements

* [CMake](https://cmake.org/download/) is required for all platforms.
* [NDK r16b](https://developer.android.com/ndk/downloads/older_releases.html) is required for Android.
* Xcode is required for macOS/iOS.
* Visual Studio 2017 and [Perl](https://www.activestate.com/products/activeperl/downloads/) is required for Windows.

## Steps

There are several build script named as `build_xxx` under each library's folder. he name of script tells the platform it builds for. Please run the scripts in the order:

1. openssl
2. nghttp2
3. curl
4. curl-unity