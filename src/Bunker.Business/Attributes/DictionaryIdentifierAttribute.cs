using System;

namespace Bunker.Business.Attributes
{
    public class DictionaryIdentifierAttribute : Attribute
    {
        public int    Identifier { get; set; }
        public string Name       { get; set; }
    }
}