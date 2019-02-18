import * as ts from "typescript";
import * as Lint from "tslint";


let ProgramClasses: (ts.ClassDeclaration | ts.InterfaceDeclaration)[] = null;

export class Rule extends Lint.Rules.TypedRule {
    public static FAILURE_STRING =
        "Derived class must has the same method signature with base class";
    public static FAILURE_DERIVED_OVERRIDE_STRING =
        "Derived method must has @override doc tag";
    public static FAILURE_DERIVED_NEW_STRING =
        "Derived method must has @new doc tag";

    public applyWithProgram(sourceFile: ts.SourceFile, program: ts.Program): Lint.RuleFailure[] {
        if (!ProgramClasses) {
            ProgramClasses = [];
            for (let sourceFile of program.getSourceFiles()) {
                if (sourceFile.fileName.indexOf("core/src") >= 0 ||
                    sourceFile.fileName.indexOf("source/map") >= 0 ||
                    sourceFile.fileName.indexOf("source/financial") >= 0 ||
                    sourceFile.fileName.indexOf("source/trellis") >= 0) {
                    if (!sourceFile.fileName.endsWith("RenderEngine.ts")) {
                        ProgramClasses.push(...getClasses(sourceFile));
                    }
                }
            }


        }
        return this.applyWithWalker(new Walk(sourceFile, this.getOptions(), ProgramClasses));
    }
}

class Walk extends Lint.RuleWalker {
    private readonly classes: (ts.ClassDeclaration | ts.InterfaceDeclaration)[];

    constructor(sourceFile: ts.SourceFile, options: Lint.IOptions, programClasses: (ts.ClassDeclaration | ts.InterfaceDeclaration)[]) {
        super(sourceFile, options);

        this.classes = programClasses;
    }

    protected visitClassDeclaration(node: ts.ClassDeclaration): void {
        let SyntaxKind = ts.SyntaxKind;
        for (let member of node.members) {
            if (member.kind == SyntaxKind.MethodDeclaration) {
                let derivedMethod = member as ts.MethodDeclaration;
                if (this.hasModify("private", derivedMethod)) {
                    continue;
                }

                let base = this.getBase(node, derivedMethod);
                if (base != null) {
                    let baseClass = base.class;
                    let baseMethod = base.method;
                    let sameSignature = this.isSameSignature(derivedMethod, baseMethod);
                    let deviredJSDoc = (<any>derivedMethod).jsDoc;
                    let deviredHasNewDocTag = this.hasJsDocTag("new", deviredJSDoc);
                    let deviredHasOverrideDocTag = this.hasJsDocTag("override", deviredJSDoc);

                    if (baseClass.kind == SyntaxKind.InterfaceDeclaration || this.hasModify("abstract", baseMethod)) {
                        if (!sameSignature) {
                            this.addFailure(this.createFailure(derivedMethod.getStart(), derivedMethod.getWidth(), Rule.FAILURE_STRING));
                        }
                    } else if (baseClass.kind == SyntaxKind.ClassDeclaration) {
                        if (sameSignature) {
                            if (this.hasModify("abstract", baseMethod)) {
                                if (!deviredHasOverrideDocTag) {
                                    this.addFailure(this.createFailure(derivedMethod.getStart(), derivedMethod.getWidth(), Rule.FAILURE_DERIVED_OVERRIDE_STRING, this.getFix("override", deviredJSDoc)));
                                }
                            } else {
                                if (!deviredHasNewDocTag) {
                                    this.addFailure(this.createFailure(derivedMethod.getStart(), derivedMethod.getWidth(), Rule.FAILURE_DERIVED_NEW_STRING, this.getFix("new", deviredJSDoc)));
                                }
                            }
                        } else {
                            if (this.isSameParameters(derivedMethod.parameters, baseMethod.parameters) &&
                                this.isSameTypeParameters(derivedMethod.typeParameters, baseMethod.typeParameters) &&
                                !deviredHasNewDocTag) {
                                this.addFailure(this.createFailure(derivedMethod.getStart(), derivedMethod.getWidth(), Rule.FAILURE_DERIVED_NEW_STRING, this.getFix("new", deviredJSDoc)));
                            }
                        }
                    } else {
                        break;
                    }
                }
            }
        }

        super.visitClassDeclaration(node);
    }

    private hasModify(name: string, method: ts.MethodDeclaration | ts.MethodSignature) {
        let modifiers = method.modifiers;
        if (!modifiers) {
            return false;
        }

        for (let modify of modifiers) {
            if (modify.getText() == name) {
                return true;
            }
        }
        return false;
    }

    private hasJsDocTag(name: string, jsDoc: any): boolean {
        let jsDocComment: ts.JSDoc = jsDoc && jsDoc[0];
        if (!jsDocComment) {
            return false;
        }
        let jsDocTags: ts.NodeArray<ts.JSDocTag> = jsDocComment.tags;
        if (!jsDocTags || jsDocTags.length == 0) {
            return false;
        }

        for (let tag of jsDocTags) {
            if (tag.tagName.getText() == name) {
                return true;
            }
        }
        return false;
    }

    private getFix(tagName: string, jsDoc: any): Lint.Replacement {
        let jsDocComment: ts.JSDoc = jsDoc && jsDoc[0];
        if (!jsDocComment) {
            return null;
        }

        return Lint.Replacement.appendText(jsDocComment.getEnd() - 2, "* @" + tagName + "\r\n\t\t ");
    }

    private getBase(node: ts.ClassDeclaration, method: ts.MethodDeclaration): { class: ts.ClassDeclaration | ts.InterfaceDeclaration, method: ts.MethodDeclaration | ts.MethodSignature } {
        let heritageClauses: ts.NodeArray<ts.HeritageClause> = node.heritageClauses;
        if (!heritageClauses || heritageClauses.length == 0) {
            return null;
        }

        let queue: ts.HeritageClause[] = [];
        for (let i = 0; i < heritageClauses.length; i++) {
            queue.unshift(heritageClauses[i]);
        }
        while (queue.length > 0) {
            let heritage = queue.pop();
            let heritageText = heritage.getText().replace("implements", "").replace("extends", "");
            for (let baseClassName of heritageText.split(",")) {
                baseClassName = this.trimClassName(baseClassName.trim());
                let baseClass = this.getClass(baseClassName);

                if (baseClass != null) {
                    let baseMethod = this.getMethod(method.name.getText(), baseClass);
                    if (baseMethod == null) {
                        let nextHeritageClauses = baseClass.heritageClauses;
                        if (nextHeritageClauses != null && nextHeritageClauses.length > 0) {
                            for (let i = 0; i < nextHeritageClauses.length; i++) {
                                queue.unshift(nextHeritageClauses[i]);
                            }
                        }
                    } else {
                        return { class: baseClass, method: baseMethod };
                    }
                }
            }
        }
        return null;
    }

    private getClass(name: string): ts.ClassDeclaration | ts.InterfaceDeclaration {
        let nameParts = name.split(".");
        for (let node of this.classes) {
            let fullName = this.trimClassName(this.getFullName(node));

            let fullNameParts = fullName.split(".");
            fullNameParts = fullNameParts.slice(fullNameParts.length - Math.min(fullNameParts.length, nameParts.length));
            if (nameParts.join() == fullNameParts.join()) {
                return node;
            }
        }
        return null;
    }

    private getMethod(name: string, node: ts.ClassDeclaration | ts.InterfaceDeclaration): ts.MethodDeclaration | ts.MethodSignature {
        let SyntaxKind = ts.SyntaxKind;
        for (let member of node.members) {
            if (member.kind == SyntaxKind.MethodDeclaration && member.name.getText() == name) {
                return member as ts.MethodDeclaration;
            }
            if (member.kind == SyntaxKind.MethodSignature && member.name.getText() == name) {
                return member as ts.MethodSignature;
            }
        }
        return null;
    }

    private getFullName(node: ts.ClassDeclaration | ts.InterfaceDeclaration): string {
        let name: string = node.name.getText();
        let parent: ts.Node = node.parent;
        while (parent) {
            if (parent.kind == ts.SyntaxKind.ModuleDeclaration) {
                name = (parent as ts.ModuleDeclaration).name.getText() + "." + name;
            }
            parent = parent.parent;
        }
        return name;
    }

    private isSameSignature(method1: ts.MethodDeclaration, method2: ts.MethodDeclaration | ts.MethodSignature): boolean {
        if (method1.name.getText() != method2.name.getText()) {
            return false;
        }
        if (!this.isSameType(method1.type, method2.type)) {
            return false;
        }
        if (!this.isSameParameters(method1.parameters, method2.parameters)) {
            return false;
        }
        if (!this.isSameTypeParameters(method1.typeParameters, method2.typeParameters)) {
            return false;
        }
        return true;
    }

    private isSameParameters(parameters1: ts.NodeArray<ts.ParameterDeclaration>, parameters2: ts.NodeArray<ts.ParameterDeclaration>) {
        if (parameters1.length != parameters2.length) {
            return false;
        }
        for (let i = 0; i < parameters1.length; i++) {
            if (!this.isSameType(parameters1[i].type, parameters2[i].type)) {
                return false;
            }
            if (parameters1[i].dotDotDotToken != parameters2[i].dotDotDotToken) {
                return false;
            }
        }
        return true;
    }

    private isSameType(type1: ts.TypeNode, type2: ts.TypeNode) {
        if (type1 && type2) {
            let typeText1: string = type1.getText();
            let typeText2: string = type2.getText();
            typeText1 = this.trimTypeName(typeText1);
            typeText2 = this.trimTypeName(typeText2);

            return (
                typeText1 == typeText2 ||
                typeText1 == "T" ||
                typeText2 == "T" ||
                typeText1.endsWith("<T>") ||
                typeText2.endsWith("<T>"));
        } else if ((!type1 && type2) || (type1 && !type2)) {
            return false;
        } else {
            return true;
        }
    }

    private trimTypeName(type: string): string {
        return type
            .replace("dv.", "")
            .replace("core.", "")
            .replace("financial.", "")
            .replace("map.", "")
            .replace("options.", "")
            .replace("models.", "");
    }

    private trimClassName(name: string): string {
        let genericIndex = name.indexOf("<");
        if (genericIndex >= 0) {
            return name.substring(0, genericIndex);
        }
        return name;
    }

    private isSameTypeParameters(typeParameters1: ts.NodeArray<ts.TypeParameterDeclaration>, typeParameters2: ts.NodeArray<ts.TypeParameterDeclaration>) {
        if (typeParameters1 && typeParameters2) {
            if (typeParameters1.length != typeParameters2.length) {
                return false;
            }
            for (let i = 0; i < typeParameters1.length; i++) {
                if (typeParameters1[i].getText() != typeParameters2[i].getText()) {
                    return false;
                }
            }
            return true;
        } else if ((!typeParameters1 && typeParameters2) || (typeParameters1 && !typeParameters2)) {
            return false;
        } else {
            return true;
        }
    }

}

function getClasses(node: ts.Node): (ts.ClassDeclaration | ts.InterfaceDeclaration)[] {
    let ret: (ts.ClassDeclaration | ts.InterfaceDeclaration)[] = [];
    let SyntaxKind = ts.SyntaxKind;
    switch (node.kind) {
        case SyntaxKind.SourceFile:
            let sourceFile = node as ts.SourceFile;
            for (let st of sourceFile.statements) {
                ret.push(...getClasses(st));
            }
            break;

        case SyntaxKind.ModuleDeclaration:
            let moduleDeclaration = node as ts.ModuleDeclaration;
            ret.push(...getClasses(moduleDeclaration.body));
            break;

        case SyntaxKind.ModuleBlock:
            let moduleBlock = node as ts.ModuleBlock;
            for (let st of moduleBlock.statements) {
                ret.push(...getClasses(st));
            }
            break;

        case SyntaxKind.ClassDeclaration:
            ret.push(node as ts.ClassDeclaration);
            break;

        case SyntaxKind.InterfaceDeclaration:
            ret.push(node as ts.InterfaceDeclaration);
            break;

        default:
            break;
    }
    return ret;
}
