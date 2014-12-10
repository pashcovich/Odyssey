#region Using Directives

using System;

#endregion

namespace Odyssey.Content
{
    [Flags]
    public enum AssetOperation
    {
        None = 0,
        Merge = 1 << 0,
    }
}