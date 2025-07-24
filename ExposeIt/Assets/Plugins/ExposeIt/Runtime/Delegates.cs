using System;

namespace ExposeIt.Runtime
{
    public delegate void CreateFunctionDelegate(string functionName, Action function);

    public delegate void DisposeFunctionDelegate(string functionName);
}
