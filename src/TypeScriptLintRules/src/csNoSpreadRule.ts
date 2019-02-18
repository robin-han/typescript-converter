import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Spread operator is forbidden, consider replacing it.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    let SyntaxKind = ts.SyntaxKind;

    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        switch (node.kind) {
            case SyntaxKind.SpreadElement:
            case SyntaxKind.SpreadAssignment:
                if (node.parent.kind !== SyntaxKind.CallExpression) {
                    return ctx.addFailure(node.pos, node.end, Rule.FAILURE_STRING);
                }
                break;

            default:
                break;
        }
        return ts.forEachChild(node, cb);
    });
}
