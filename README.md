# Convert TypeScript to CSharp
TypeScript to CSharp is a language convert tool, it first build typescript code to AST use typescript compiler, then convert the AST to csharp.
Now it can only convert typescript to csharp language, later it will support convert to java languate.
The tool run on dotnet2.1

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
After build, and switch to directory 'demo' and run follow command to convert.
Folder 'input' is the typescript code and 'csharp' is the converted result.

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
  "include": [],                 // source list directory or files
  "exclude": [],                 // exclude source directory or files
  "samples": [],                 // If translate part of the source, put class name here, it will convert all its referenced
  "outputs": [
    {
      "path": "",                      // output directory
      "patterns": [],                  // file pattern to math output
      "flatOutput": false,             // true will not keep its directory hierarchy
      "preferTypeScriptType": true,    // keep typescript's primary type, when true please inlcude 'TypeScriptObject' project in your solution.
      "namespace": "MyNamespace",      // output's namespace
      "usings": [                      // will add them on the head of every file 
        "System.Linq"
      ]
    }
  ],
  "namespaceMappings": [],       // map typescript's namespace
  "omittedQualifiedNames": []    // omit qualified names, eg: dv.model
}
```