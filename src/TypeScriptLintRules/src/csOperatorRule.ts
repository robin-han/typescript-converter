import * as ts from "typescript";
import * as Lint from "tslint";
import { isValidNumericLiteral } from "tsutils";

export class Rule extends Lint.Rules.TypedRule {
    public static FAILURE_STRING =
        "The operator muse be applied in bool operand";

    public applyWithProgram(sourceFile: ts.SourceFile, program: ts.Program): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk, undefined, program.getTypeChecker());
    }
}


function walk(ctx: Lint.WalkContext<void>, checker: ts.TypeChecker) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        let SyntaxKind = ts.SyntaxKind;

        if (isBinaryCheckNode(node)) {
            let binary: ts.BinaryExpression = node as ts.BinaryExpression;
            let andToken: boolean = binary.operatorToken.kind == SyntaxKind.AmpersandAmpersandToken;
            let orToken: boolean = binary.operatorToken.kind == SyntaxKind.BarBarToken;
            if (andToken || orToken) {
                let left: ts.Expression = binary.left;
                let right: ts.Expression = binary.right;
                let leftType: ts.Type = checker.getTypeAtLocation(left);
                let rightType: ts.Type = checker.getTypeAtLocation(right);
                let leftTypeString: string = checker.typeToString(leftType);
                let rightTypeString: string = checker.typeToString(rightType);

                if (leftTypeString != "boolean" && rightTypeString !== "boolean") {
                    let fix: Lint.Replacement = andToken ? getBinaryAndFix(binary, checker) : getBinaryOrFix(binary, checker);
                    let fixes: Lint.Replacement[] = [
                        fix,
                        Lint.Replacement.deleteFromTo(node.getStart(), node.getEnd())
                    ];
                    return ctx.addFailure(node.getStart(), node.getEnd(), Rule.FAILURE_STRING, fixes);
                }
            }
        }
        return ts.forEachChild(node, cb);

    });
}

function isBinaryCheckNode(node: ts.Node) {
    if (ts.isBinaryExpression(node)) {
        let SyntaxKind = ts.SyntaxKind;
        let parentKind = node.parent.kind;

        return (
            parentKind == SyntaxKind.VariableDeclaration ||
            parentKind == SyntaxKind.ExpressionStatement
        );
    }
    return false;
}

function getBinaryAndFix(binary: ts.BinaryExpression, checker: ts.TypeChecker): Lint.Replacement {
    let left: ts.Expression = binary.left;
    let right: ts.Expression = binary.right;
    let leftText: string = left.getText();
    let rightText: string = right.getText();
    let leftType: string = checker.typeToString(checker.getTypeAtLocation(left));

    let variableValue = "";
    switch (leftType) {
        case "boolean":
            variableValue = "false";
            break;
        case "number":
            variableValue = "0";
            break;
        default:
            variableValue = "null";
            break;
    }
    return Lint.Replacement.appendText(binary.getEnd(), leftText + " ? " + rightText + " : " + variableValue);
}

function getBinaryOrFix(binary: ts.BinaryExpression, checker: ts.TypeChecker): Lint.Replacement {
    let left: ts.Expression = binary.left;
    let right: ts.Expression = binary.right;
    let leftText: string = left.getText();
    let rightText: string = right.getText();

    return Lint.Replacement.appendText(binary.getEnd(), leftText + " ? " + leftText + " : " + rightText);
}

