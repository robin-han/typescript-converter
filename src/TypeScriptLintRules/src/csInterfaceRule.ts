import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public static FAILURE_STRING =
        "Interface should has only one member of call signature.";

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk);
    }
}


function walk(ctx: Lint.WalkContext<void>) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (node.kind === ts.SyntaxKind.InterfaceDeclaration) {
            let interfaceNode: ts.InterfaceDeclaration = node as ts.InterfaceDeclaration;
            let members: ts.NodeArray<ts.TypeElement> = interfaceNode.members;
            let signatureMember = members.find(mem => mem.kind === ts.SyntaxKind.CallSignature);

            if (signatureMember != null && members.length > 1) {
                return ctx.addFailure(signatureMember.pos, signatureMember.end, Rule.FAILURE_STRING);
            }
        }
        return ts.forEachChild(node, cb);
    });
}