#!/usr/bin/env bash

set -exuo pipefail

UNITY_VERSION="$(awk -F': ' '/m_EditorVersion: / { print $2 }'< ProjectSettings/ProjectVersion.txt)"
case "$OSTYPE" in
    darwin*)
        UNITY="/Applications/Unity/Hub/Editor/${UNITY_VERSION}/Unity.app/Contents/MacOS/Unity"
        ;;
    msys*)
        UNITY="/c/Program Files/Unity/Hub/Editor/${UNITY_VERSION}/Editor/Unity.exe"
        ;;
    linux*)
        UNITY="$HOME/Unity/Hub/Editor/${UNITY_VERSION}/Editor/Unity"
        export ANDROID_NDK_ROOT="$HOME/Unity/Hub/Editor/${UNITY_VERSION}/Editor/Data/PlaybackEngines/AndroidPlayer/NDK"
        ;;
    default)
        echo "unknown os: $OSTYPE"
        exit 1
        ;;
esac

BASE_DIR="$(pwd)"
RUN_UNITY=("${UNITY}" "-logFile" "-" "-batchmode" "-nographics" "-projectPath" "${BASE_DIR}")
#export RUST_LOG=info

clean() {
    rm -rf Library out
}

prepare() {
    TY="$1"
    BUILDOUT="out/${TY}"
    rm -rf "${BUILDOUT}" && mkdir -p "${BUILDOUT}"
}

android() {
    prepare android
    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.Android
}

android_il2cpp() {
    prepare android_il2cpp
    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.AndroidIL2CPP
}

android_il2cpp_store() {
     prepare android_il2cpp
     "${RUN_UNITY[@]}" \
         -quit -executeMethod BuildScript.AndroidIL2CPPStore
}

ios() {
    prepare ios
    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.iOS
}

macos() {
    prepare macos

    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.MacOS
}

linux() {
    prepare linux

    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.Linux
}

windows() {
    prepare windows

    "${RUN_UNITY[@]}" \
        -quit -executeMethod BuildScript.Windows
}

editor() {
    "${UNITY}" "-logFile" "-" "-projectPath" "${BASE_DIR}"
}

until [ "$#" == 0 ]; do
    time $1
    shift
done
