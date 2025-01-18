using System.Reflection;
using DistIL.AsmIO;
using DistIL.CodeGen.Cil;
using DistIL.Frontend;
using DistIL.IR;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.Parsing;
using Socordia.Core.CompilerService;

namespace SocordiaC.Compilation;

public class CollectUnitsListener : Listener<Driver, AstNode, UnitDeclaration>
{
    private FieldDef valueField;
    private FieldDef nameField;

    protected override void ListenToNode(Driver context, UnitDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns,ParsingUtils.ToPascalCase(node.Name),
            TypeAttributes.Public, context.Compilation.Module.Resolver.Import(typeof(object)));

        type.GetCustomAttribs(false).Add(context.KnownAttributes.GetAttribute<MeasureAttribute>());

        nameField = type.CreateField("Name", new TypeSig(PrimType.String),
            FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault, node.Name);

        var valueType = Utils.GetTypeFromNode(node.Type, type);
        valueField = type.CreateField("value__", new TypeSig(valueType));

        CreateCtor(type, valueType);
        CreateToString(type);

        Utils.EmitAnnotations(node, type);
    }

    private void CreateToString(TypeDef type)
    {
        var toString = type.CreateMethod("ToString", new TypeSig(PrimType.String), [new ParamDef(new TypeSig(type), "this")], MethodAttributes.Public | MethodAttributes.Virtual);
        toString.Body = new(toString);

        var ir = new IRBuilder(toString.Body.CreateBlock());

        // Load the value field
        var fld = ir.CreateFieldAddr(valueField, toString.Body.Args[0]);

        // Call ToString on the value field
        var valueType = type.Module.Resolver.SysTypes.GetPrimitiveDef(valueField.Type.Kind);
        var valueString = ir.Emit(new CallInst(valueType.FindMethod("ToString"), [fld]));

        var name = ConstString.Create(" " + (string)nameField.DefaultValue);

        var concat = type.Module.Resolver.SysTypes.String.FindMethod("Concat", new MethodSig(PrimType.String, [PrimType.String, PrimType.String]));
        var result = ir.Emit(new CallInst(concat, [valueString, name]));

        ir.Emit(new ReturnInst(result));

        toString.ILBody = ILGenerator.GenerateCode(toString.Body);
    }

    private void CreateCtor(TypeDef type, TypeDesc valueType)
    {
        var ctor = type.CreateMethod(".ctor", new TypeSig(PrimType.Void), [new ParamDef(new TypeSig(type), "this"), new ParamDef(new TypeSig(valueType), "value")], MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public);
        ctor.Body = new(ctor);

        var ir = new IRBuilder(ctor.Body.CreateBlock());

        var objCtor = type.Module.Resolver.SysTypes.Object.FindMethod(".ctor");
        ir.CreateCall(objCtor, ctor.Body.Args[0]);

        ir.CreateFieldStore(valueField, ctor.Body.Args[0], ctor.Body.Args[1]);

        ir.Emit(new ReturnInst());

        ctor.ILBody = ILGenerator.GenerateCode(ctor.Body);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}