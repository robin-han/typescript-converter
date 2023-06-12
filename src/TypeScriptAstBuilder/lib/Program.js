#!/usr/bin/env node
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const TsAstBuilder_1 = require("./TsAstBuilder");
const FileSystem = require("fs");
const Path = require("path");
var tools;
(function (tools) {
    class Program {
        constructor(source, output) {
            this.sourcePath = Path.resolve(__dirname, source);
            this.outputFolder = Path.resolve(__dirname, output);
        }
        static main(args) {
            if (args.length != 2) {
                Program.showUsage();
            }
            else {
                let application = new Program(args[0], args[1]);
                application.run();
            }
        }
        static showUsage() {
            console.log();
            console.log('Usage: node Program.js <source file>|<source folder> <output folder>');
            console.log();
        }
        run() {
            if (FileSystem.existsSync(this.sourcePath)) {
                this.printLine('Start parsing the typescript files...');
                if (FileSystem.existsSync(this.outputFolder)) {
                    this.removeFolder(this.outputFolder);
                }
                if (FileSystem.statSync(this.sourcePath).isDirectory()) {
                    this.visitFolder(this.sourcePath);
                }
                else {
                    this.visitFile(this.sourcePath);
                }
                this.printLine('Finishing parsing.');
            }
            else {
                this.printLine('Cannot find the typescript files folder ' + this.sourcePath);
                Program.showUsage();
            }
        }
        visitFolder(path) {
            var items = FileSystem.readdirSync(path);
            for (let item of items) {
                let itemPath = Path.join(path, item);
                if (FileSystem.statSync(itemPath).isDirectory()) {
                    this.visitFolder(itemPath);
                }
                else {
                    this.visitFile(itemPath);
                }
            }
        }
        visitFile(path) {
            if (Path.extname(path) == '.ts') {
                let astFilePath = Path.join(this.outputFolder, Path.relative(this.sourcePath, path)) + '.json';
                let astFileFolder = Path.dirname(astFilePath);
                this.makeFolder(astFileFolder);
                let builder = new TsAstBuilder_1.TsAstBuilder();
                let ast = builder.build(path);
                FileSystem.writeFile(astFilePath, JSON.stringify(ast, null, 2), (error) => { if (error != null) {
                    throw error;
                } });
            }
        }
        printLine(message) {
            console.log('[' + new Date().toLocaleTimeString() + ']: ' + message);
        }
        removeFolder(path) {
            if (FileSystem.existsSync(path)) {
                let items = FileSystem.readdirSync(path);
                for (let item of items) {
                    let itemPath = Path.join(path, item);
                    if (FileSystem.statSync(itemPath).isDirectory()) {
                        this.removeFolder(itemPath);
                    }
                    else {
                        FileSystem.unlinkSync(itemPath);
                    }
                }
            }
        }
        makeFolder(path) {
            if (FileSystem.existsSync(path)) {
                return;
            }
            let parent = Path.dirname(path);
            this.makeFolder(parent);
            FileSystem.mkdirSync(path);
        }
    }
    tools.Program = Program;
})(tools || (tools = {}));
tools.Program.main(process.argv.slice(2));
