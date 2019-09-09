# TypeScriptConverter: Convert TypeScript to CSharp
TypeScript is a language convert translator tool. Now it can convert typescript language to csharp language, and convert typescript language to java language will come soon.

## Build
Windows
```bash
build.cmd
```

Linux
```bash
build.sh
```

## Demo
After build, and switch to directory demo

Windows
```bash
ts2csharp.cmd
```

Linux
```bash
ts2csharp.sh
```


## Config
All config setting in file tscconfig.json
```js
{
  "include": [], // source list directory or file
  "exclude": [], // exclude source will not translate
  "output": "", // output
  "samples": [], // If translate part of the source, put the root class name here
  "flatOutput": false, // true will not keep its directory hierarchy
  "preferTypeScriptType": true,
  "namespace": "MyNamespace",
  "usings": [
    "System.Linq"
  ],
  "namespaceMappings": [],
  "omittedQualifiedNames": [] // omit qualified names, eg: dv.model
}
```