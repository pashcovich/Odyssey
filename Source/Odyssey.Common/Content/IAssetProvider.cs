#region Using Directives

using SharpDX;
using System;

#endregion Using Directives

namespace Odyssey.Content
{
    public interface IAssetProvider
    {
        IServiceRegistry Services { get; }

        bool Contains(string assetName);

        T Get<T>(string assetName, object options = null);

        void Store<T>(string assetName, T asset);

        bool TryGetAsset(string assetName, out object asset);

        void LoadAssetList(string fileName);

        bool Unload<T>(string assetName);

        void Unload();

        Type Map(string assetType);

        void AddMapping(string key, Type assetType);
    }
}