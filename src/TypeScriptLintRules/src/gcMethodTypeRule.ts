import * as ts from "typescript";
import * as Lint from "tslint";
import { isValidNumericLiteral } from "tsutils";

export class Rule extends Lint.Rules.TypedRule {
    public static FAILURE_STRING =
        "Method must has a type";

    public applyWithProgram(sourceFile: ts.SourceFile, program: ts.Program): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk, undefined, program.getTypeChecker());
    }
}


function walk(ctx: Lint.WalkContext<void>, checker: ts.TypeChecker) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (isMethodNode(node) && !hasMethodType(node)) {
            const fixType = getFixType(node, checker);
            const appendPos = getAppendTypePos(node);
            const fix = fixType ? Lint.Replacement.appendText(appendPos, ': ' + fixType) : undefined;

            return ctx.addFailure(node.getStart(), node.getEnd(), Rule.FAILURE_STRING, fix);
        }
        return ts.forEachChild(node, cb);
    });
}

function isMethodNode(node: ts.Node) {
    let SyntaxKind = ts.SyntaxKind;

    switch (node.kind) {
        case SyntaxKind.MethodSignature:
        case SyntaxKind.MethodDeclaration:
        case SyntaxKind.FunctionDeclaration:
            return true;

        default:
            return false;
    }
}

function hasMethodType(node: ts.Node) {

    let signature = node as ts.SignatureDeclarationBase;
    return (signature != null && signature.type != null);
}

function getFixType(node: ts.Node, checker: ts.TypeChecker): string {
    let type: ts.Type = checker.getTypeAtLocation(node);
    if (type != null) {
        let typeString: string = checker.typeToString(type);
        let index: number = typeString.lastIndexOf("=>");
        if (index >= 0) {
            return typeString.substring(index + 2).trim();
        }
    }
    return null;
}

function getAppendTypePos(node: ts.Node): number {
    let SyntaxKind = ts.SyntaxKind;

    switch (node.kind) {
        case SyntaxKind.MethodSignature:
            return node.end - 1;

        case SyntaxKind.MethodDeclaration:
            return (node as ts.MethodDeclaration).body.pos;

        case SyntaxKind.FunctionDeclaration:
            return (node as ts.FunctionDeclaration).body.pos;

        default:
            return -1;
    }
}

