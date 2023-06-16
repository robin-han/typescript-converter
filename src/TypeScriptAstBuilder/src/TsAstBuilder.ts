import * as fs from 'fs';
import * as typescript from 'typescript';

namespace tools {
    export class TsAstBuilder {
        public build(fileName: string): object {
            let sourceFileNode = typescript.createSourceFile(fileName, fs.readFileSync(fileName).toString(), typescript.ScriptTarget.ES2015, true);

            return this._toJson(sourceFileNode);
        }

        protected _toJson(node: typescript.Node): object {
            let json: { [key: string]: any } = {};
            for (let key in node) {
                if (key == 'parent') {
                    continue;
                } else if (key == 'kind') {
                    json['kind'] = typescript.SyntaxKind[node.kind].toString();
                }
                else {
                    let value = (node as any)[key];
                    if ((value == null) ||
                        (typeof value === 'function')) {
                    } else if ((typeof value === 'string') ||
                        (typeof value === 'boolean') ||
                        (typeof value === 'number')) {
                        json[key] = value;
                    } else if (Array.isArray(value)) {
                        json[key] = [];
                        for (let item of value) {
                            if ((typeof item === 'string') ||
                                (typeof item === 'boolean') ||
                                (typeof item === 'number')) {
                                json[key].push(item);
                            } else {
                                json[key].push(this._toJson(item));
                            }
                        }
                    } else if (typeof value === 'object') {
                        json[key] = this._toJson(value);
                    } else {
                        console.log(key + ':' + value + ':');
                        throw 'Unexcepted type is found!';
                    }
                }
            }
            return json;
        }
    }
}

export const TsAstBuilder = tools.TsAstBuilder;