using System;
using System.Reflection;
using Bunker.Business.Attributes;
using Bunker.Business.Entities.Dictioneries;

namespace Bunker.Business.Extensions
{
    public static class RoleDictionaryExtension
    {
        public static int Identifier(this RoleDictionary roleDictionary)
        {
            var attribute = roleDictionary.GetType().GetCustomAttribute<DictionaryIdentifierAttribute>();

            if (attribute == null)
                throw new ArgumentException();

            return attribute.Identifier;
        }

        public static string Name(this RoleDictionary roleDictionary)
        {
            var attribute = roleDictionary.GetType().GetCustomAttribute<DictionaryIdentifierAttribute>();

            if (attribute == null)
                throw new ArgumentException();

            return attribute.Name;
        }
    }
}