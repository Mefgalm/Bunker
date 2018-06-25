using System;

namespace Bunker.Business.Internal.Interfaces.Services
{
    public interface IMefMapper
    {
        TOut Map<TIn, TOut>(TIn inValue);

        void AddMap<TIn, TOut>(Func<TIn, TOut> func);
    } 
}