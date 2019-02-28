cmake_minimum_required(VERSION 2.8)

set(PROJ_ROOT ${CMAKE_CURRENT_SOURCE_DIR})

set(PROJ curl-unity)
project(${PROJ})

if (WIN32)
    set(ARCH win)
elseif (APPLE)
    if (IOS)
        set(ARCH ios)
    else()
        set(ARCH osx)
    endif()
elseif (ANDROID)
    set(ARCH android/${ANDROID_ABI})
endif ()

include_directories(${PROJ_ROOT}/../curl/prebuilt/${ARCH}/include)
link_directories( 
    ${PROJ_ROOT}/../curl/prebuilt/${ARCH}/lib
    ${PROJ_ROOT}/../openssl/prebuilt/${ARCH}/lib
    ${PROJ_ROOT}/../nghttp2/prebuilt/${ARCH}/lib
    )

set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++14")

set(SOURCES "curl-unity.cpp")
if (WIN32)
    set(LIBS libcurl_a libssl libcrypto nghttp2 ws2_32 wldap32 crypt32)
else()
    set(LIBS curl ssl crypto nghttp2 z)
endif()

set(OUTPUT_LIB ${PROJ})

if (APPLE)
    if (IOS)
        set(CMAKE_OSX_ARCHITECTURES "$(ARCHS_STANDARD)")
        add_library(${OUTPUT_LIB} STATIC ${SOURCES})
        set_xcode_property (${OUTPUT_LIB} IPHONEOS_DEPLOYMENT_TARGET "12.0" "all")
    else()
        set(CMAKE_OSX_ARCHITECTURES "$(ARCHS_STANDARD_64_BIT)")
        add_library(${OUTPUT_LIB} MODULE ${SOURCES})
        set_target_properties (${OUTPUT_LIB} PROPERTIES BUNDLE TRUE )
    endif()
else()
    add_library(${OUTPUT_LIB} SHARED ${SOURCES})
endif()

add_definitions("-DCURL_STATICLIB")

target_link_libraries(${OUTPUT_LIB} ${LIBS})
install(TARGETS ${PROJ} 
    LIBRARY DESTINATION ${CMAKE_INSTALL_PREFIX}
    ARCHIVE DESTINATION ${CMAKE_INSTALL_PREFIX}
    RUNTIME DESTINATION ${CMAKE_INSTALL_PREFIX}
    BUNDLE DESTINATION ${CMAKE_INSTALL_PREFIX})

 if (NOT IOS)
    set(TEST ${PROJ}_test)
    add_executable(${TEST} "test.cpp")
    target_link_libraries(${TEST} ${LIBS})
 endif()