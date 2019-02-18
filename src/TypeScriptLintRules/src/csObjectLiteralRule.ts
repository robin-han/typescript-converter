import * as ts from "typescript";
import * as Lint from "tslint";
import { isValidNumericLiteral } from "tsutils";

export class Rule extends Lint.Rules.TypedRule {
    public static FAILURE_STRING =
        "Object literal at least has two elements";

    public applyWithProgram(sourceFile: ts.SourceFile, program: ts.Program): Lint.RuleFailure[] {
        return this.applyWithFunction(sourceFile, walk, undefined, program.getTypeChecker());
    }
}


function walk(ctx: Lint.WalkContext<void>, checker: ts.TypeChecker) {
    return ts.forEachChild(ctx.sourceFile, function cb(node: ts.Node): void {
        if (node.kind === ts.SyntaxKind.ObjectLiteralExpression) {
            let objectLiteral = node as ts.ObjectLiteralExpression;

            if (!isValidObjectLiteral(objectLiteral, checker)) {
                return ctx.addFailure(node.pos, node.end, Rule.FAILURE_STRING);
            }
        }
        return ts.forEachChild(node, cb);
    });
}

function isValidObjectLiteral(node: ts.ObjectLiteralExpression, checker: ts.TypeChecker): boolean {
    if (node.properties.length >= 2) {
        return true;
    }

    let SyntaxKind = ts.SyntaxKind;
    let type: ts.Type = checker.getContextualType(node);

    if (type == null) {
        return false;
    }

    let typeNode: ts.TypeNode = checker.typeToTypeNode(type);
    switch (typeNode.kind) {
        case SyntaxKind.TypeLiteral:
            return isValidTypeLiteral(typeNode as ts.TypeLiteralNode);
        case SyntaxKind.TypeReference:
            return isValidTypeReference(type);
        case SyntaxKind.AnyKeyword:
            return true;
        default:
            return false;
    }
}

function isValidTypeLiteral(type: ts.TypeLiteralNode): boolean {
    let members = type.members;
    if (members.length === 1 && members[0].kind === ts.SyntaxKind.IndexSignature) {
        return true;
    }
    return false;
}

function isValidInterfaceDeclaration(interfaceDeclaration: ts.InterfaceDeclaration): boolean {
    let members = interfaceDeclaration.members;
    if (members.length === 1 && members[0].kind === ts.SyntaxKind.IndexSignature) {
        return true;
    }
    return false;
}

function isValidTypeReference(nodeType: ts.Type): boolean {
    let SyntaxKind = ts.SyntaxKind;
    let declaration = nodeType.symbol.getDeclarations()[0];

    if (declaration == null) {
        return false;
    }

    switch (declaration.kind) {
        case SyntaxKind.TypeLiteral:
            return isValidTypeLiteral(declaration as ts.TypeLiteralNode);

        case SyntaxKind.InterfaceDeclaration:
            return isValidInterfaceDeclaration(declaration as ts.InterfaceDeclaration);

        default:
            return false;
    }
}

