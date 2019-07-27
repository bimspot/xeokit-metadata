#!/bin/bash

RuntimeIdentifiers=(
  linux-x64
  linux-musl-x64
  linux-arm
  win-x64
  win-x86
  win-arm
  win-arm64
  osx-x64)

printf "Publishing Framework-dependent executable.\n\n"

for id in "${RuntimeIdentifiers[@]}"
do
  printf "[x] Packaging for runtime: $id\n\n"

  dotnet publish ../xeokit/xeokit-metadata/xeokit-metadata.csproj \
    -c Release \
    -r $id \
    --self-contained false \
    --output ../../dist/$id

  # zip -r $id.zip $id
  tar -zcvf $id.tar.gz $id
  rm -rf $id

  printf "\n\n"
done

printf "Publishing completed.\n\n"