import { TsAstBuilder } from './TsAstBuilder';
import * as FileSystem from 'fs';
import * as Path from 'path';

namespace tools {
    export class Program {
        public static main(args: string[]): void {
            if (args.length != 2) {
                Program.showUsage();
            } else {
                let application = new Program(args[0], args[1])
                application.run();
            }
        }
        public static showUsage(): void {
            console.log();
            console.log('Usage: node Program.js <source file>|<source folder> <output folder>');
            console.log();
        }

        private sourcePath: string;
        private outputFolder: string;

        constructor(source:string, output:string) {
            this.sourcePath = Path.resolve(__dirname, source);
            this.outputFolder = Path.resolve(__dirname, output);
        }

        protected run(): void {
            if (FileSystem.existsSync(this.sourcePath)) {
                this.printLine('Start parsing the typescript files...');

                if (FileSystem.existsSync(this.outputFolder)) {
                    this.removeFolder(this.outputFolder);
                }

                if (FileSystem.statSync(this.sourcePath).isDirectory()) {
                    this.visitFolder(this.sourcePath)
                } else {
                    this.visitFile(this.sourcePath);
                }

                this.printLine('Finishing parsing.');
            }
            else {
                this.printLine('Cannot find the typescript files folder ' + this.sourcePath);
                Program.showUsage();
            }
        }

        protected visitFolder(path:string): void {
            var items = FileSystem.readdirSync(path);

            for (let item of items) {
                let itemPath = Path.join(path, item);
                if (FileSystem.statSync(itemPath).isDirectory()) {
                    this.visitFolder(itemPath);
                } else {
                    this.visitFile(itemPath);
                }
            }
        }
        protected visitFile(path:string): void {
            if (Path.extname(path) == '.ts') {
                let astFilePath = Path.join(this.outputFolder, Path.relative(this.sourcePath, path)) + '.json';
                let astFileFolder = Path.dirname(astFilePath);
                this.makeFolder(astFileFolder);

                let builder = new TsAstBuilder();
                let ast = builder.build(path);
                FileSystem.writeFile(astFilePath, JSON.stringify(ast, null, 2), (error) => { if (error != null) { throw error; } });
            }
        }
        protected printLine(message:string): void {
            console.log('[' + new Date().toLocaleTimeString() + ']: ' + message);
        }

        private removeFolder(path:string): void {
            if (FileSystem.existsSync(path)) {
                let items = FileSystem.readdirSync(path);
                for (let item of items) {
                    let itemPath = Path.join(path, item);
                    if (FileSystem.statSync(itemPath).isDirectory()) {
                        this.removeFolder(itemPath);
                    } else {
                        FileSystem.unlinkSync(itemPath);
                    }
                }
            }
        }
        private makeFolder(path:string): void {
            if (FileSystem.existsSync(path)) {
                return;
            }

            let parent = Path.dirname(path);
            this.makeFolder(parent);
            FileSystem.mkdirSync(path);
        }
    }
}

tools.Program.main(process.argv.slice(2));


