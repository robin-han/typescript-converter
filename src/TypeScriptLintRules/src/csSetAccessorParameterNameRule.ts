import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Set accessor's parameter name must be 'value'.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    let SyntaxKind = ts.SyntaxKind;

    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (node.kind === SyntaxKind.SetAccessor) {
            let accessor = node as ts.SetAccessorDeclaration;
            let parameter = accessor.parameters[0];
           
            if (parameter != null && parameter.name.getText() !== 'value') {
                return ctx.addFailure(parameter.pos, parameter.end, Rule.FAILURE_STRING);
            }
        }
        return ts.forEachChild(node, cb);
    });
}
