import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Parameter should not has modify.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (node.kind === ts.SyntaxKind.Parameter) {
            let parameter = node as ts.ParameterDeclaration;
            let modifiers = parameter.modifiers;

            if (modifiers != null && modifiers.length > 0) {
                let fixes: Lint.Replacement[] = [];
                for (let modify of modifiers) {
                    fixes.push(Lint.Replacement.deleteFromTo(modify.getStart(), modify.getEnd()));
                }

                return ctx.addFailure(node.pos, node.end, Rule.FAILURE_STRING, fixes);
            }
        }
        return ts.forEachChild(node, cb);
    });
}
