# arcade-light demo project
Arcade-light is inspired from [dotnet/arcade](https://github.com/dotnet/arcade) but minimized and without Microsoft proprietary tooling


## How to use [DotNetDev.ArcadeLight.Sdk](https://github.com/Bertk/arcade-light/tree/main/src/DotNetDev.ArcadeLight.Sdk)

### Configuration

#### 1) add global.json

```json
{
  "sdk": {
    "version": "8.0.404",
    "rollForward": "latestFeature"
  },
  "tools": {
    "dotnet": "8.0.404"
  },
  "msbuild-sdks": {
    "DotNetDev.ArcadeLight.Sdk": "1.8.0"
  }
}
```

#### 2) Add lines in Directory.Build.props

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.props" Sdk="DotNetDev.ArcadeLight.Sdk" />

  <PropertyGroup>
      <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  ...
<\Project>
```

#### 3) Add line in Directory.build.targets

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.targets" Sdk="DotNetDev.ArcadeLight.Sdk" />
  ...
<\Project>
```

#### 4) Add lines in Directory.Packages.props

```xml
<Project>
  <PropertyGroup>
    <CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
  </PropertyGroup>

  <ItemGroup>
    <GlobalPackageReference Include="Nerdbank.GitVersioning" Version="3.7.112" />
  </ItemGroup>
  ...
<\Project>
```

#### 5) Copy `eng\commonlight` from Arcade-light into repo.

#### 6) Edit  `eng\common-variables.yml` and update variable `_SolutionFile` value

#### 7) Add the Versions.props file to your eng\ folder

#### 8) copy `version.json` to repository root folder

#### 9) optionally copy the scripts for `restore`, `build` and `test` to repository root folder

### Use ArcadeLight with command shell or Visual Studio

```shell
restore
build
test
```

## build script help: `build -h`

```text
Common settings:
  -configuration <value>  Build configuration: 'Debug' or 'Release' (short: -c)
  -platform <value>       Platform configuration: 'x86', 'x64' or any valid Platform value to pass to msbuild
  -verbosity <value>      Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)
  -binaryLog              Output binary log (short: -bl)
  -help                   Print help and exit

Actions:
  -restore                Restore dependencies (short: -r)
  -build                  Build solution (short: -b)
  -rebuild                Rebuild solution
  -test                   Run all unit tests in the solution (short: -t)
  -clean                  Clean the solution

Advanced settings:
  -projects <value>       Semi-colon delimited list of sln/proj's to build. Globbing is supported (*.sln)
  -ci                     Set when running on CI server
  -excludeCIBinarylog     Don't output binary log (short: -nobl)
  -prepareMachine         Prepare machine for CI run, clean up processes after build
  -msbuildEngine <value>  Msbuild engine to use to run build ('dotnet', 'vs', or unspecified).
  -excludePrereleaseVS    Set to exclude build engines in prerelease versions of Visual Studio

Command line arguments not listed above are passed thru to msbuild.
The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.).
```
