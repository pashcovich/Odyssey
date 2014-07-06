using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Content.Meshes;
using Odyssey.Content.Models;
using Odyssey.UserInterface.Style;

namespace Odyssey.Content
{
    public static class ContentMapper
    {
        private static readonly Dictionary<AssetType, Type> contentMapper = new Dictionary<AssetType, Type>()
        {
            {AssetType.ControlDefinitions , typeof(ControlDescription[])},
            {AssetType.TextDefinitions, typeof(TextDescription[])},
            {AssetType.Model, typeof(Model)}

        };

        public static Type Map(AssetType type)
        {
            return contentMapper[type];
        }
    }
}
