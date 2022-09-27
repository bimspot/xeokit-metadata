#!/bin/bash

RuntimeIdentifiers=(
  linux-x64
  linux-musl-x64
  linux-arm
  linux-arm64
  win-x64
  win-x86
  win-arm
  win-arm64
  osx-x64
  osx.11.0-arm64)

printf "Publishing Framework-dependent executable.\n\n"

for id in "${RuntimeIdentifiers[@]}"
do
  printf "[x] Packaging for runtime: $id\n\n"

  dotnet publish ../xeokit/xeokit-metadata/xeokit-metadata.csproj \
    -c Release \
    -r $id \
    --self-contained false \
    --output xeokit-metadata-$id

  # zip -r $id.zip $id
  tar -zcvf xeokit-metadata-$id.tar.gz xeokit-metadata-$id
  rm -rf xeokit-metadata-$id

  printf "\n\n"
done

printf "Publishing completed.\n\n"