using Odyssey.Engine;
using System;

namespace Odyssey.Content
{
    /// <summary>
    /// Base class for all GraphicsResource content reader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class ResourceReaderBase<T> : IContentReader
    {
        object IContentReader.ReadContent(IAssetProvider readerManager, ref ContentReaderParameters parameters)
        {
            parameters.KeepStreamOpen = false;
            var service = readerManager.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (service == null)
                throw new InvalidOperationException("Unable to retrieve a IDirectXDeviceService service provider");

            if (service.DirectXDevice == null)
                throw new InvalidOperationException("DirectXDevice is not initialized");

            return ReadContent(readerManager, service.DirectXDevice, ref parameters);
        }

        protected abstract T ReadContent(IAssetProvider readerManager, DirectXDevice device, ref ContentReaderParameters parameters);
    }
}