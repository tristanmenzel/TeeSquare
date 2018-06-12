using System;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class FunctionWriter : ICodePart
    {
        private readonly MethodInfo _method;
        private bool _useArrows;

        public FunctionWriter(string name)
        {
            _method = new MethodInfo(name);
        }

        public FunctionWriter WithReturnType(string returnType, params string[] genericTypeParams)
        {
            _method.WithReturnType(returnType, genericTypeParams);
            return this;
        }

        public FunctionWriter WithGenericTypeParams(params string[] genericTypeParams)
        {
            _method.WithGenericTypeParams(genericTypeParams);
            return this;
        }


        public FunctionWriter WithBody(Action<ICodeWriter> writeBody)
        {
            _method.WithBody(writeBody);
            return this;
        }

        public FunctionWriter WithParams(Action<IParamConfigurator> configureParams)
        {
            _method.WithParams(configureParams);
            return this;
        }
        
        public FunctionWriter AsConstArrows()
        {
            _useArrows = true;
            return this;
        }

        void ICodePart.WriteTo(ICodeWriter writer)
        {
            if (_useArrows)
            {
                writer.Write($"export const {_method.Id.Name} = ");
            }
            else
            {
                writer.Write($"export function {_method.Id.Name}");
            }

            if (_method.Id.GenericTypeParams.Any())
            {
                writer.Write("<");
                writer.Write(string.Join(", ", _method.Id.GenericTypeParams));
                writer.Write(">");
            }
            writer.Write("(");
            

            writer.WriteDelimited(_method.Params,
                (p, w) =>
                {
                    w.Write($"{p.Name}: ");
                    w.WriteType(p.Type, p.GenericTypeParams);
                }, ", ");

            writer.Write("): ");
            writer.WriteType(_method.ReturnType.Type, _method.ReturnType.GenericTypeParams);

            writer.Write(_useArrows ? " =>" : "");
            writer.OpenBrace();

            _method.WriteBody(writer);

            writer.CloseBrace(_useArrows);
        }

        /*
         * export const blah = <T1>(p1: P1, p2: P2) => {
         *
         *
         * function blah<T>(p1: P1, p2: P2) {
         * 
         */
    }
}