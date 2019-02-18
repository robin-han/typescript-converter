import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Function cannot be defined in global. Consider defined in a class.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithWalker(new Walk(sourceFile, this.getOptions()));
    }
}


class Walk extends Lint.RuleWalker {
    public visitFunctionDeclaration(node: ts.FunctionDeclaration): void {
        const SyntaxKind = ts.SyntaxKind;

        switch (node.parent.kind) {
            case SyntaxKind.ModuleBlock:
            case SyntaxKind.ModuleDeclaration:
                this.addFailure(this.createFailure(node.getStart(), node.getWidth(), Rule.FAILURE_STRING));
                break;
            default:
                break;
        }

        super.visitFunctionDeclaration(node);
    }

}
