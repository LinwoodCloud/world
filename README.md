
# Linwood World

> Free, opensource, modular, adventure voxel game

[![Discord badge](https://img.shields.io/discord/735424757142519848?style=for-the-badge)](https://discord.linwood.tk)
[![GitHub repo size](https://img.shields.io/github/repo-size/LinwoodCloud/world?style=for-the-badge)](https://github.com/LinwoodCloud/world/archive/main.zip)

## Building

- Download the Godot Engine 3.3.1 - Mono [here](https://godotengine.org/download)
- Download the dependencies <https://docs.godotengine.org/en/stable/development/compiling/index.html>
- Install all submodules with `git submodule update --init` in the root folder and in the `godot-cpp` submodule.
- Build the bindings with (Platform can be `windows`, `linux` and `osx`):

  ```bash
    cd godot-cpp
    scons platform=<platform> generate_bindings=yes -j4 use_custom_api_file=yes custom_api_file=../api.json
    cd ..
  ```

- Open the project with godot

## Installation

### Mobile

#### Android

- [Stable](https://github.com/LinwoodCloud/world/releases/download/release/app-release.apk)
- [Preview](https://github.com/LinwoodCloud/world/releases/download/preview/app-release.apk)

##### Stores

> Currently not available.

###### Community

> Currently not available.

#### iOS

> Currently not available.

### Web

- [Stable](https://world.linwood.tk)
- [Preview](https://preview.world.linwood.tk)

### Desktop

#### Windows

- [Stable](https://github.com/LinwoodCloud/world/releases/download/release/windows.zip)
- [Preview](https://github.com/LinwoodCloud/world/releases/download/preview/windows.zip)

#### Linux

- [Stable](https://github.com/LinwoodCloud/world/releases/download/release/linux.zip)
- [Preview](https://github.com/LinwoodCloud/world/releases/download/preview/linux.zip)

#### MacOS

> Currently not available.

## Contributing

![Locale chart](https://badges.awesome-crowdin.com/translation-200008942-8.png)

### Issues

If you find bugs or have feature request, you can create an issue by clicking [here](https://github.com/LinwoodCloud/world/issues/new/choose).

### Pull request

If you want to help me to code, you can open a pull request. Fork this project and make a pull request. I only accept pull request for the *develop* branch.

### Community Server

A project without a community isn't a good project. You can [join the discord](https://discord.linwood.tk) and talk to each other!

### Branches

| Name    | Support |                                                                                Description |
| :------ | :-----: | -----------------------------------------------------------------------------------------: |
| main    |    ✅    | This branch is production ready. It will be updated when a new stable version is released! |
| develop |    ❌    |    This branch is only for testing and developing. Pull request should only be added here! |
