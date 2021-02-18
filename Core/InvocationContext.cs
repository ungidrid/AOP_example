using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Core.Abstract;
using Core.Attributes;

namespace Core
{
    public class InvocationContext
    {
        internal InterceptionAttribute Attribute { get; set; }
        internal AspectBase Interceptor { get; set; }
        internal IInvocation Invocation { get; set; }
        internal IServiceProvider ServiceProvider { get; set; }
        internal bool InvocationIsBypassed { get; set; }
        internal ConcurrentDictionary<string, object> TempData { get; set; }

        public InvocationContext() => TempData = new ConcurrentDictionary<string, object>();

        public InterceptionAttribute GetOwningAttribute() => Attribute;

        public Type GetOwningType() => Invocation.Method.DeclaringType;

        public IServiceProvider GetServiceProvider() => ServiceProvider;

        public T GetParameterValue<T>(int parameterPosition) => (T)Invocation.GetArgumentValue(parameterPosition);

        public object GetParameterValue(int parameterPosition) => Invocation.GetArgumentValue(parameterPosition);

        public void SetParameterValue(int parameterPosition, object newValue)
        {
            Invocation.SetArgumentValue(parameterPosition, newValue);
        }

        public void SetTemporaryData(string name, object value)
        {
            TempData.TryAdd(name, value);
        }

        public object GetTemporaryData(string name) => TempData.GetValueOrDefault(name);

        public object GetMethodReturnValue() => Invocation.ReturnValue;

        public void OverrideMethodReturnValue(object returnValue)
        {
            Invocation.ReturnValue = returnValue;
        }

        public void BypassInvocation()
        {
            InvocationIsBypassed = true;
        }

        public MethodInfo GetExecutingMethodInfo() => Invocation.Method;

        public string GetExecutingMethodName() => Invocation.Method.Name;

        public int GetParameterPosition(Type typeToFind)
        {
            var method = Invocation.Method;
            if(method == null)
                throw new ArgumentNullException(nameof(method));

            for(var i = method.GetParameters().Length - 1; i >= 0; i--)
            {
                var paramType = method.GetParameters()[i]
                    .ParameterType;

                if(paramType != typeToFind)
                    continue;

                return i;
            }

            return -1;
        }

        public int GetParameterPosition<TTypeToFind>() => GetParameterPosition(typeof(TTypeToFind));
    }
}