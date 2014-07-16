using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using Odyssey.Engine;

namespace Odyssey.Daedalus.Data
{
    internal static class ReferenceSerializer
    {
        static internal void Serialize(string fullPath)
        {

            var dRefs = new[]
            {
                ReferenceFactory.Application.ViewportSize,
                ReferenceFactory.Camera.MatrixProjection,
                ReferenceFactory.Camera.MatrixView,
                ReferenceFactory.Camera.VectorPosition,
                ReferenceFactory.Effect.BloomParameters,
                ReferenceFactory.Effect.BloomThreshold,
                ReferenceFactory.Effect.BlurOffsetsWeights,
                ReferenceFactory.Effect.BlurOffsetsWeights,
                ReferenceFactory.Effect.SpritePosition,
                ReferenceFactory.Effect.SpriteSize,
                ReferenceFactory.Entity.MatrixWorld,
                ReferenceFactory.Entity.MatrixWorldInverse,
                ReferenceFactory.Entity.MatrixWorldInverseTranspose,
                ReferenceFactory.Light.MatrixProjection,
                ReferenceFactory.Light.MatrixView,
                ReferenceFactory.Light.StructPointPS,
                ReferenceFactory.Light.StructPointVS,
                ReferenceFactory.Light.VectorDirection,
                ReferenceFactory.Material.ColorDiffuse,
                ReferenceFactory.Material.StructMaterialPS
            };

            var serializer = new DataContractSerializer(typeof (EngineReference[]));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };
            using (var fs = new FileStream(fullPath, FileMode.Create))
            using (var writer = XmlWriter.Create(fs, settings))
            {
                serializer.WriteObject(writer, dRefs);
            }

        }
    }
}
