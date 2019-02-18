import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.TypedRule {
    public static FAILURE_STRING =
        "Variable declaration must has type.";

    public applyWithProgram(sourceFile: ts.SourceFile, program: ts.Program): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk, undefined, program.getTypeChecker());
    }
}

function walk(ctx: Lint.WalkContext<void>, checker: ts.TypeChecker): void {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        const SyntaxKind = ts.SyntaxKind;

        switch (node.kind) {
            case SyntaxKind.Parameter:
            case SyntaxKind.PropertyDeclaration:
            case SyntaxKind.VariableDeclaration:
                const { type, name } = <any>node;
                if (type == null && needType(node)) {

                    const fixType = getFixType(node, checker);
                    const fix = fixType ? Lint.Replacement.appendText(name.end, ': ' + fixType) : undefined;

                    ctx.addFailure(node.getStart(), node.getEnd(), Rule.FAILURE_STRING, fix);
                }
                break;
            default:
                break;
        }
        return ts.forEachChild(node, cb);
    });
}

function getFixType(node: ts.Node, checker: ts.TypeChecker): string {
    try {
        const { initializer } = <any>node;
        if (isBasicValueType(initializer)) {
            return getBasicValueType(initializer);
        }

        let nodeType: ts.Type = checker.getTypeAtLocation(node);
        if (nodeType != null) {
            return checker.typeToString(nodeType);
        }
    } catch { }

    return undefined;
}

function needType(node: ts.Node) {
    //Lint.Rules.TypedRule
    const SyntaxKind = ts.SyntaxKind;
    const { parent, initializer } = <any>node;

    if (node.kind === SyntaxKind.Parameter && parent.kind === SyntaxKind.ArrowFunction) {
        return false;
    }
    if (parent.parent.kind === SyntaxKind.ForInStatement || parent.parent.kind === SyntaxKind.ForOfStatement) {
        return false;
    }
    if (parent.kind === SyntaxKind.CatchClause) {
        return false;
    }

    if (initializer == null) {
        return true;
    }
    if (initializer.kind === SyntaxKind.NewExpression) {
        return false;
    }

    return true;
}

function isBasicValueType(initializer: ts.Expression): boolean {
    if (initializer == null) {
        return false;
    }

    const SyntaxKind = ts.SyntaxKind;
    //ts.TypeChecker
    switch (initializer.kind) {
        case SyntaxKind.NumericLiteral:
        case SyntaxKind.StringLiteral:
        case SyntaxKind.TrueKeyword:
        case SyntaxKind.FalseKeyword:
            return true;

        case SyntaxKind.PrefixUnaryExpression:
            let nd = initializer as ts.PrefixUnaryExpression
            if (nd.operand.kind === SyntaxKind.NumericLiteral) {
                return true;
            }
            return false;

        default:
            return false;
    }
}

function getBasicValueType(initializer: ts.Expression): string {
    const SyntaxKind = ts.SyntaxKind;
    switch (initializer.kind) {
        case SyntaxKind.NumericLiteral:
            return "number";

        case SyntaxKind.StringLiteral:
            return "string";

        case SyntaxKind.TrueKeyword:
        case SyntaxKind.FalseKeyword:
            return "boolean";

        case SyntaxKind.PrefixUnaryExpression:
            let nd = initializer as ts.PrefixUnaryExpression
            if (nd.operand.kind === SyntaxKind.NumericLiteral) {
                return "number";
            }
            return undefined;

        default:
            return undefined;
    }
}
