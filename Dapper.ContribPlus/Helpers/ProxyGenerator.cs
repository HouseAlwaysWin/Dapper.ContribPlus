using Dapper.ContribPlus.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Dapper.ContribPlus.Helpers
{
    internal static class ProxyGenerator
    {
        private static readonly Dictionary<Type, Type> TypeCache = new Dictionary<Type, Type>();

        public static AssemblyBuilder GetAsmBuilder(string name)
        {
#if NETSTANDARD2_0
            return AssemblyBuilder.DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
#else
                return Thread.GetDomain().DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
#endif
        }

        public static T GetInterfaceProxy<T>()
        {
            Type typeOfT = typeof(T);

            if (TypeCache.TryGetValue(typeOfT, out Type k))
            {
                return (T)Activator.CreateInstance(k);
            }
            var assemblyBuilder = GetAsmBuilder(typeOfT.Name);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("SqlMapperExtensions." + typeOfT.Name); //NOTE: to save, add "asdasd.dll" parameter

            var interfaceType = typeof(IProxy);
            var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "_" + Guid.NewGuid(),
                TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(typeOfT);
            typeBuilder.AddInterfaceImplementation(interfaceType);

            //create our _isDirty field, which implements IProxy
            var setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);

            // Generate a field for each property, which implements the T
            foreach (var property in typeof(T).GetProperties())
            {
                var isId = property.GetCustomAttributes(true).Any(a => a is KeyAttribute);
                CreateProperty<T>(typeBuilder, property.Name, property.PropertyType, setIsDirtyMethod, isId);
            }

#if NETSTANDARD2_0
            var generatedType = typeBuilder.CreateTypeInfo().AsType();
#else
                var generatedType = typeBuilder.CreateType();
#endif

            TypeCache.Add(typeOfT, generatedType);
            return (T)Activator.CreateInstance(generatedType);
        }

        public static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
        {
            var propType = typeof(bool);
            var field = typeBuilder.DefineField("_" + nameof(IProxy.IsDirty), propType, FieldAttributes.Private);
            var property = typeBuilder.DefineProperty(nameof(IProxy.IsDirty),
                                           System.Reflection.PropertyAttributes.None,
                                           propType,
                                           new[] { propType });

            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.SpecialName
                                              | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig;

            // Define the "get" and "set" accessor methods
            var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + nameof(IProxy.IsDirty),
                                         getSetAttr,
                                         propType,
                                         Type.EmptyTypes);
            var currGetIl = currGetPropMthdBldr.GetILGenerator();
            currGetIl.Emit(OpCodes.Ldarg_0);
            currGetIl.Emit(OpCodes.Ldfld, field);
            currGetIl.Emit(OpCodes.Ret);
            var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + nameof(IProxy.IsDirty),
                                         getSetAttr,
                                         null,
                                         new[] { propType });
            var currSetIl = currSetPropMthdBldr.GetILGenerator();
            currSetIl.Emit(OpCodes.Ldarg_0);
            currSetIl.Emit(OpCodes.Ldarg_1);
            currSetIl.Emit(OpCodes.Stfld, field);
            currSetIl.Emit(OpCodes.Ret);

            property.SetGetMethod(currGetPropMthdBldr);
            property.SetSetMethod(currSetPropMthdBldr);
            var getMethod = typeof(IProxy).GetMethod("get_" + nameof(IProxy.IsDirty));
            var setMethod = typeof(IProxy).GetMethod("set_" + nameof(IProxy.IsDirty));
            typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
            typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);

            return currSetPropMthdBldr;
        }

        private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod, bool isIdentity)
        {
            //Define the field and the property 
            var field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
            var property = typeBuilder.DefineProperty(propertyName,
                                           System.Reflection.PropertyAttributes.None,
                                           propType,
                                           new[] { propType });

            const MethodAttributes getSetAttr = MethodAttributes.Public
                                                | MethodAttributes.Virtual
                                                | MethodAttributes.HideBySig;

            // Define the "get" and "set" accessor methods
            var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName,
                                         getSetAttr,
                                         propType,
                                         Type.EmptyTypes);

            var currGetIl = currGetPropMthdBldr.GetILGenerator();
            currGetIl.Emit(OpCodes.Ldarg_0);
            currGetIl.Emit(OpCodes.Ldfld, field);
            currGetIl.Emit(OpCodes.Ret);

            var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                                         getSetAttr,
                                         null,
                                         new[] { propType });

            //store value in private field and set the isdirty flag
            var currSetIl = currSetPropMthdBldr.GetILGenerator();
            currSetIl.Emit(OpCodes.Ldarg_0);
            currSetIl.Emit(OpCodes.Ldarg_1);
            currSetIl.Emit(OpCodes.Stfld, field);
            currSetIl.Emit(OpCodes.Ldarg_0);
            currSetIl.Emit(OpCodes.Ldc_I4_1);
            currSetIl.Emit(OpCodes.Call, setIsDirtyMethod);
            currSetIl.Emit(OpCodes.Ret);

            //TODO: Should copy all attributes defined by the interface?
            if (isIdentity)
            {
                var keyAttribute = typeof(KeyAttribute);
                var myConstructorInfo = keyAttribute.GetConstructor(new Type[] { });
                var attributeBuilder = new CustomAttributeBuilder(myConstructorInfo, new object[] { });
                property.SetCustomAttribute(attributeBuilder);
            }

            property.SetGetMethod(currGetPropMthdBldr);
            property.SetSetMethod(currSetPropMthdBldr);
            var getMethod = typeof(T).GetMethod("get_" + propertyName);
            var setMethod = typeof(T).GetMethod("set_" + propertyName);
            typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
            typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);
        }
    }
}
