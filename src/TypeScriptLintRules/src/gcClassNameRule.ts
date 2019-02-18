import { isClassLikeDeclaration, isInterfaceDeclaration } from "tsutils";
import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING = "Class name must be in pascal case";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}

function walk(ctx: Lint.WalkContext<void>) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (
            (isClassLikeDeclaration(node) && node.name !== undefined) ||
            isInterfaceDeclaration(node)
        ) {
            if (!isPascalCased(node.name!.text)) {
                ctx.addFailureAtNode(node.name!, Rule.FAILURE_STRING);
            }
        }
        return ts.forEachChild(node, cb);
    });
}

function isPascalCased(name: string): boolean {
    if (name[0] == '_') {
        name = name.substring(1);
    }
    return isUpperCase(name[0]) && !name.includes("_") && !name.includes("-");
}

function isUpperCase(str: string): boolean {
    return str === str.toUpperCase();
}