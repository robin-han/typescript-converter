import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "FunctionType is forbidden, consider replacing it with interface.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (isForbitFunctionType(node)) {
            return ctx.addFailure(node.pos, node.end, Rule.FAILURE_STRING);
        }
        return ts.forEachChild(node, cb);
    });
}

function isForbitFunctionType(node: ts.Node): boolean {
    if (node.kind !== ts.SyntaxKind.FunctionType) {
        return false;
    }

    let SyntaxKind = ts.SyntaxKind;
    let parent: ts.Node = node.parent;

    switch (parent.kind) {
        case SyntaxKind.TypeAliasDeclaration:
        case SyntaxKind.InterfaceDeclaration:
            return false;

        default:
            return true;
    }
}
