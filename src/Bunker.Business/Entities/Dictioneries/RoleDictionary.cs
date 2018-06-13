using System;
using System.Reflection;

namespace Bunker.Business.Entities.Dictioneries
{
    public enum RoleDictionary
    {
        [DictionaryIdentifier(Identifier = 1, Name = "Init")]
        Init,
    }

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

    public class DictionaryIdentifierAttribute : Attribute
    {
        public int    Identifier { get; set; }
        public string Name       { get; set; }
    }
}