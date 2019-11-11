﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.Mobx
{
    public interface IMobxModelWriterFactory
    {
        CodeSnippetWriter BuildModel(IComplexTypeInfo typeInfo);

        CodeSnippetWriter BuildHeader();
    }

    internal static class MobxTypeExtensions{
        internal static string MakeOptional(this string input, bool isOptional)
        {
            return isOptional ? $"types.maybe({input})" : input;
        }
        internal static string MakeCollection(this string input, bool isCollection)
        {
            return isCollection ? $"types.array({input})" : input;
        }
    }

    public class MobxModelWriterFactory : IMobxModelWriterFactory
    {
        private IDictionary<string, string> _mobxTypes = new Dictionary<string, string>()
        {
            {"number", "types.number"},
            {"string", "types.string"},
            {"boolean", "types.boolean"},
        };

        protected virtual string GetMobxType(ITypeReference type)
        {
            return GetMobxBaseType(type)
                .MakeCollection(type.Array)
                .MakeOptional(type.Optional);
        }
        protected virtual string GetMobxBaseType(ITypeReference type)
        {
            if (type.Enum)
            {
                return $"types.enumeration<{type.TypeName}>(\"{type.TypeName}\", [Object.values({type.TypeName})])";
            }

            return type.TypeName switch
            {
                "number" => type.Format == TsTypeFormat.Integer ? "types.integer" : "types.number",
                "string" => type.Format == TsTypeFormat.DateTime ? "types.Date" : "types.string",
                "boolean" => "types.boolean",
                _ => $"{type.TypeName}Model"
            };
        }

        public CodeSnippetWriter BuildModel(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.OpenBlock($"export const {typeInfo.Name}Model = types.model('{typeInfo.Name}Model', ");
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}: ", true);
                    writer.Write(GetMobxType(prop.Type));
                    writer.WriteLine(",", false);
                }

                writer.CloseBlock("});");
                writer.WriteLine("");
                writer.WriteLine($"export type {typeInfo.Name} = Instance<typeof {typeInfo.Name}Model>;");
            };
        }


        public CodeSnippetWriter BuildHeader()
        {
            return writer =>
            {
                writer.WriteLine("import { types, Instance } from 'mobx-state-tree';");
                writer.WriteLine("");
            };
        }
    }
}