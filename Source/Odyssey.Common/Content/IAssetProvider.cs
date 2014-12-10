#region Using Directives

using System;
using System.Collections.Generic;
using Odyssey.Core;

#endregion

namespace Odyssey.Content
{
    public interface IAssetProvider
    {
        IServiceRegistry Services { get; }

        bool Contains(string assetName);

        T Load<T>(string assetName, object options = null);

        IEnumerable<T> GetAll<T>();

        void Store<T>(string assetName, T asset);

        bool TryGetAsset(string assetName, out object asset);

        void LoadAssetList(string fileName);

        bool Unload<T>(string assetName);

        void Unload();

        Type Map(string assetType);

        void AddMapping(string key, Type assetType);

        event EventHandler<AssetsLoadedEventArgs> AssetsLoaded;
    }
}