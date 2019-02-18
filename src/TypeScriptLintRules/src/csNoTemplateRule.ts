import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Template strings is forbidden, consider replacing it.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (node.kind === ts.SyntaxKind.TemplateExpression) {
            return ctx.addFailure(node.pos, node.end, Rule.FAILURE_STRING);
        }
        return ts.forEachChild(node, cb);
    });
}
