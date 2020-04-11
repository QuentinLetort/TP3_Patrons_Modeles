using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace AppWithPlugin
{
    public static class TypeExtensions
    {
        public static bool IsAssignableFromGeneric(this Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return baseType.IsAssignableFromGeneric(genericType);
        }


    }
}
