using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Assimp;
using Assimp.Unmanaged;
using Odyssey.Graphics;
using Odyssey.Graphics.Models;
using Odyssey.Text.Logging;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Mathematics;
using Mesh = Assimp.Mesh;

namespace Odyssey.Tools.Compiler.Model
{
    public class ModelCompiler
    {
        private Scene scene;
        private ModelData model;
        private ModelCompilerOptions options;
        private string modelFilePath;
        private string modelDirectory;
        private List<ModelData.MeshPart>[] registeredMeshParts;
        private readonly Dictionary<Node, int> meshNodes = new Dictionary<Node, int>();

        private Vector3[] boundingPoints;
        private int currentBoundingPointIndex;
        private int boundingPointCount;

        public static ContentCompilerResult CompileFromFile(string fileName, ModelCompilerOptions compilerOptions)
        {
            var modelCompiler = new ModelCompiler();
            return modelCompiler.CompileFromFileInternal(fileName, compilerOptions);
        }

        public static ContentCompilerResult Compile(Stream modelStream, string fileName, ModelCompilerOptions compilerOptions)
        {
            var compiler = new ModelCompiler();
            return compiler.CompileFromStream(modelStream, fileName, compilerOptions);
        }

        private ContentCompilerResult CompileFromStream(Stream modelStream, string fileName, ModelCompilerOptions compilerOptions)
        {
            var result = new ContentCompilerResult();
            options = compilerOptions;
            modelFilePath = fileName;
            modelDirectory = Path.GetDirectoryName(modelFilePath);

            // Preload AssimpLibrary if not already loaded
            if (!AssimpLibrary.Instance.LibraryLoaded)
            {
                var rootPath = Path.GetDirectoryName(typeof(AssimpLibrary).Assembly.Location);
                AssimpLibrary.Instance.LoadLibrary(Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath32Bit), Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath64Bit));
            }

            var importer = new AssimpImporter();
            //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            // Steps for Direct3D, should we make this configurable?
            var steps = PostProcessSteps.FlipUVs | PostProcessSteps.FlipWindingOrder;

            // Setup quality
            switch (compilerOptions.Quality)
            {
                case ModelRealTimeQuality.Low:
                    steps |= PostProcessPreset.TargetRealTimeFast;
                    break;
                case ModelRealTimeQuality.Maximum:
                    steps |= PostProcessPreset.TargetRealTimeMaximumQuality;
                    break;
                default:
                    steps |= PostProcessPreset.TargetRealTimeQuality;
                    break;
            }

            scene = importer.ImportFileFromStream(modelStream, steps, Path.GetExtension(fileName));
            model = new ModelData();
            ProcessScene();

            result.IsContentGenerated = true;
            result.ModelData = model;

            return result;
        }

        public static ContentCompilerResult CompileAndSave(string fileName, string outputFile, ModelCompilerOptions compilerOptions)
        {
            Contract.Requires<ArgumentNullException>(fileName != null, "fileName");
            Contract.Requires<ArgumentNullException>(outputFile != null, "outputFile");
            Contract.Requires<ArgumentNullException>(compilerOptions != null, "compilerOptions");
           
            bool contentToUpdate = true;
            if (compilerOptions.DependencyFile != null)
            {
                if (!FileDependencyList.CheckForChanges(compilerOptions.DependencyFile))
                {
                    contentToUpdate = false;
                }
            }

            var result = new ContentCompilerResult ();
            if (contentToUpdate)
            {
                try
                {
                    result = CompileFromFile(fileName, compilerOptions);

                    if (result.HasErrors)
                    {
                        return result;
                    }

                    var modelData = result.ModelData;

                    // Make sure that directory name doesn't collide with filename
                    var directoryName = Path.GetDirectoryName(outputFile + ".tmp");
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Save the model
                    modelData.Save(outputFile);

                    if (compilerOptions.DependencyFile != null)
                    {
                        // Save the dependency
                        var dependencyList = new FileDependencyList();
                        dependencyList.AddDefaultDependencies();
                        dependencyList.AddDependencyPath(fileName);
                        dependencyList.Save(compilerOptions.DependencyFile);
                    }

                    result.IsContentGenerated = true;
                }
                catch (Exception ex)
                {
                    result.HasErrors = true;
                    LogEvent.Tool.Error("Unexpected exception while converting {0} : {1}", fileName, ex.ToString());
                }
            }


            return result;
        }

        private ContentCompilerResult CompileFromFileInternal(string fileName, ModelCompilerOptions compilerOptions)
        {
            var result = new ContentCompilerResult();
            options = compilerOptions;
            modelFilePath = fileName;
            modelDirectory = Path.GetDirectoryName(modelFilePath);

            // Preload AssimpLibrary if not already loaded
            if (!AssimpLibrary.Instance.LibraryLoaded)
            {
                var rootPath = Path.GetDirectoryName(typeof(AssimpLibrary).Assembly.Location);
                AssimpLibrary.Instance.LoadLibrary(Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath32Bit), Path.Combine(rootPath, AssimpLibrary.Instance.DefaultLibraryPath64Bit));
            }

            var importer = new AssimpImporter();
            //importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            // Steps for Direct3D Right-Handed, should we make this configurable?
            var steps = PostProcessSteps.FlipUVs | PostProcessSteps.FlipWindingOrder;

            // Setup quality
            switch (compilerOptions.Quality)
            {
                case ModelRealTimeQuality.Low:
                    steps |= PostProcessPreset.TargetRealTimeFast;
                    break;
                case ModelRealTimeQuality.Maximum:
                    steps |= PostProcessPreset.TargetRealTimeMaximumQuality;
                    break;
                default:
                    steps |= PostProcessPreset.TargetRealTimeQuality;
                    break;
            }

            scene = importer.ImportFile(fileName, steps);
            model = new ModelData();
             
            ProcessScene();

            result.IsContentGenerated = true;
            result.ModelData = model;

            return result;
        }

        private void ProcessScene()
        {
            registeredMeshParts = new List<ModelData.MeshPart>[scene.MeshCount];

            // Collect meshes and attached nodes
            CollectMeshNodes(scene.RootNode);

            // Collect nodes
            CollectNodes(scene.RootNode);

        }
        
        private void CollectMeshNodes(Node node)
        {
            if (node.HasMeshes)
            {
                RegisterNode(node, meshNodes);
            }

            if (node.HasChildren)
            {
                foreach (var child in node.Children)
                {
                    CollectMeshNodes(child);
                }
            }
        }

        private void RegisterNode(Node node, Dictionary<Node, int> nodeMap)
        {
            while (node != null)
            {
                if (!nodeMap.ContainsKey(node))
                {
                    nodeMap.Add(node, 0);
                }
                else
                {
                    break;
                }

                node = node.Parent;
            }
        }

        private bool IsModelNode(Node node)
        {
            // Disable Skinned bones for this version
            //return meshNodes.ContainsKey(node) || skinnedBones.ContainsKey(node);
            return meshNodes.ContainsKey(node);
        }

        private void CollectNodes(Node node)
        {
            bool isModelNode = IsModelNode(node);

            if (!isModelNode)
            {
                return;
            }

            // if node has meshes, create a new scene object for it
            if (node.MeshCount > 0)
            {
                var mesh = new ModelData.Mesh
                {
                    Name = node.Name,
                    MeshParts = new List<ModelData.MeshPart>()
                };
                model.Meshes.Add(mesh);

                // Precalculate the number of vertices for bounding sphere calculation
                boundingPointCount = 0;
                for (int i = 0; i < node.MeshCount; i++)
                {
                    var meshIndex = node.MeshIndices[i];
                    var meshPart = scene.Meshes[meshIndex];
                    boundingPointCount += meshPart.VertexCount;
                }

                // Reallocate the buffer if needed
                if (boundingPoints == null || boundingPoints.Length < boundingPointCount)
                {
                    boundingPoints = new Vector3[boundingPointCount];
                }

                currentBoundingPointIndex = 0;
                for (int i = 0; i < node.MeshCount; i++)
                {
                    var meshIndex = node.MeshIndices[i];
                    var meshPart = Process(mesh, scene.Meshes[meshIndex]);

                    var meshToPartList = registeredMeshParts[meshIndex];
                    if (meshToPartList == null)
                    {
                        meshToPartList = new List<ModelData.MeshPart>();
                        registeredMeshParts[meshIndex] = meshToPartList;
                    }

                    meshToPartList.Add(meshPart);
                    mesh.MeshParts.Add(meshPart);
                }

                // Calculate the bounding sphere.
                BoundingSphere.FromPoints(boundingPoints, 0, boundingPointCount, out mesh.BoundingSphere);
            }

            // continue for all child nodes
            if (node.HasChildren)
            {
                foreach (var subNode in node.Children)
                {
                    CollectNodes(subNode);
                }
            }
        }

        private ModelData.MeshPart Process(ModelData.Mesh mesh, Mesh assimpMesh)
        {
            var meshPart = new ModelData.MeshPart
            {
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                VertexBufferRange = new ModelData.BufferRange { Slot = mesh.VertexBuffers.Count },
                IndexBufferRange = new ModelData.BufferRange { Slot = mesh.IndexBuffers.Count }
            };

            var vertexBuffer = new ModelData.VertexBuffer
            {
                Layout = new List<VertexElement>()
            };
            mesh.VertexBuffers.Add(vertexBuffer);

            var indexBuffer = new ModelData.IndexBuffer();
            mesh.IndexBuffers.Add(indexBuffer);

            var layout = vertexBuffer.Layout;

            int vertexBufferElementSize = 0;

            // Add position
            layout.Add(VertexElement.PositionTransformed(Format.R32G32B32_Float, 0));
            vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector3>();

            // Add normals
            if (assimpMesh.HasNormals)
            {
                layout.Add(VertexElement.Normal(0, Format.R32G32B32_Float, vertexBufferElementSize));
                vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector3>();
            }

            // Add colors
            if (assimpMesh.VertexColorChannelCount > 0)
            {
                for (int localIndex = 0, i = 0; i < assimpMesh.VertexColorChannelCount; i++)
                {
                    if (assimpMesh.HasVertexColors(i))
                    {
                        layout.Add(VertexElement.Normal(localIndex, Format.R32G32B32A32_Float, vertexBufferElementSize));
                        vertexBufferElementSize += SharpDX.Utilities.SizeOf<Color4>();
                        localIndex++;
                    }
                }
            }
            
            // Add textures
            if (assimpMesh.TextureCoordsChannelCount > 0)
            {
                for (int localIndex = 0, i = 0; i < assimpMesh.TextureCoordsChannelCount; i++)
                {
                    if (assimpMesh.HasTextureCoords(i))
                    {
                        var uvCount = assimpMesh.GetUVComponentCount(i);

                        if (uvCount == 2)
                        {
                            layout.Add(VertexElement.TextureCoordinate(localIndex, Format.R32G32_Float, vertexBufferElementSize));
                            vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector2>();
                        }
                        else if (uvCount == 3)
                        {
                            layout.Add(VertexElement.TextureCoordinate(localIndex, Format.R32G32B32_Float, vertexBufferElementSize));
                            vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector3>();
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected uv count");
                        }

                        localIndex++;
                    }
                }
            }

            if (options.ModelOperations.HasFlag(ModelOperation.CalculateBarycentricCoordinates))
            {
                layout.Add(new VertexElement("BARYCENTRIC", 0, Format.R32G32B32_Float, vertexBufferElementSize));
                vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector3>();
            }
            else 
            // Add tangent / bitangent
            if (assimpMesh.HasTangentBasis)
            {
                if (!options.ExcludeElements.Contains("Tangent"))
                {
                    layout.Add(VertexElement.Tangent(Format.R32G32B32A32_Float, vertexBufferElementSize));
                    vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector4>();
                }

                if (!options.ExcludeElements.Contains("BiTangent"))
                {
                    layout.Add(VertexElement.BiTangent(Format.R32G32B32_Float, vertexBufferElementSize));
                    vertexBufferElementSize += SharpDX.Utilities.SizeOf<Vector3>();
                }
            }

            if (options.ModelOperations.HasFlag(ModelOperation.CalculateBarycentricCoordinates))
            {

                WriteBarycentricVertices(assimpMesh, meshPart, vertexBuffer, vertexBufferElementSize);
            }
            else
            {
                WriteVertices(assimpMesh, meshPart, vertexBuffer, vertexBufferElementSize);
            }
            WriteIndices(assimpMesh, meshPart, indexBuffer);
            return meshPart;
        }

        void WriteBarycentricVertices(Mesh assimpMesh, ModelData.MeshPart meshPart, ModelData.VertexBuffer vertexBuffer, int vertexBufferElementSize)
        {
            //System.Diagnostics.Debugger.Launch();
            int[] indices = assimpMesh.GetIntIndices();
            int indexCount = indices.Length;
            // Write all vertices
            meshPart.VertexBufferRange.Count = indexCount;
            vertexBuffer.Count = indexCount;
            vertexBuffer.Buffer = new byte[vertexBufferElementSize * indexCount];
            
            // Update the MaximumBufferSizeInBytes needed to load this model
            if (vertexBuffer.Buffer.Length > model.MaximumBufferSizeInBytes)
            {
                model.MaximumBufferSizeInBytes = vertexBuffer.Buffer.Length;
            }

            var vertexStream = DataStream.Create(vertexBuffer.Buffer, true, true);
            
            boundingPoints = new Vector3[indexCount];
            var newVertices = new VertexPositionNormalTexture[indices.Length]; 
            for (int i = 0; i < indexCount; i++)
            {
                try
                {
                    int i0 = indices[i];
                    var v0 = assimpMesh.Vertices[i0];
                    var v = new Vector3(v0.X, v0.Y, v0.Z);

                    var n0 = assimpMesh.Normals[i0];
                    var n = new Vector3(n0.X, n0.Y, n0.Z);

                    var uv = assimpMesh.GetTextureCoords(0)[i0];
                    var t = new Vector2(uv.X, uv.Y);

                    // Store bounding points for BoundingSphere pre-calculation
                    boundingPoints[currentBoundingPointIndex++] = v;
                    newVertices[i0] = new VertexPositionNormalTexture(v, n, t);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("!" + ex);
                }
            }
            var barycentricVertices = ModelEditor.ConvertToBarycentricEdgeNormalVertices(newVertices, indices);
            foreach (var vertex in barycentricVertices)
            {
                vertexStream.Write(vertex.Position);
                vertexStream.Write(vertex.Normal);
                vertexStream.Write(vertex.TextureUV);
                vertexStream.Write(vertex.Barycentric);
            }

            vertexStream.Dispose();
        }

        void WriteVertices(Mesh assimpMesh, ModelData.MeshPart meshPart, ModelData.VertexBuffer vertexBuffer, int vertexBufferElementSize)
        {
            // Write all vertices
            meshPart.VertexBufferRange.Count = assimpMesh.VertexCount;
            vertexBuffer.Count = assimpMesh.VertexCount;
            vertexBuffer.Buffer = new byte[vertexBufferElementSize * assimpMesh.VertexCount];

            // Update the MaximumBufferSizeInBytes needed to load this model
            if (vertexBuffer.Buffer.Length > model.MaximumBufferSizeInBytes)
            {
                model.MaximumBufferSizeInBytes = vertexBuffer.Buffer.Length;
            }

            var vertexStream = DataStream.Create(vertexBuffer.Buffer, true, true);
            for (int i = 0; i < assimpMesh.VertexCount; i++)
            {
                var position = assimpMesh.Vertices[i];
                vertexStream.Write(position);

                // Store bounding points for BoundingSphere pre-calculation
                boundingPoints[currentBoundingPointIndex++] = new Vector3(position.X, position.Y, position.Z);

                // Add normals
                if (assimpMesh.HasNormals)
                {
                    vertexStream.Write(assimpMesh.Normals[i]);
                }

                // Add colors
                if (assimpMesh.VertexColorChannelCount > 0)
                {
                    for (int j = 0; j < assimpMesh.VertexColorChannelCount; j++)
                    {
                        if (assimpMesh.HasVertexColors(j))
                        {
                            vertexStream.Write(assimpMesh.GetVertexColors(j)[i]);
                        }
                    }
                }

                // Add textures
                if (assimpMesh.TextureCoordsChannelCount > 0)
                {
                    for (int j = 0; j < assimpMesh.TextureCoordsChannelCount; j++)
                    {
                        if (assimpMesh.HasTextureCoords(j))
                        {
                            var uvCount = assimpMesh.GetUVComponentCount(j);

                            var uv = assimpMesh.GetTextureCoords(j)[i];

                            if (uvCount == 2)
                            {
                                vertexStream.Write(new Vector2(uv.X, uv.Y));
                            }
                            else
                            {
                                vertexStream.Write(uv);
                            }
                        }
                    }
                }

                // Add tangent / bitangent
                if (assimpMesh.HasTangentBasis)
                {
                    if (!options.ExcludeElements.Contains("Tangent"))
                    {
                        double w = Vector3D.Dot(assimpMesh.Normals[i],
                            Vector3D.Cross(assimpMesh.Tangents[i], assimpMesh.Tangents[i]));
                        Vector3D t = assimpMesh.Tangents[i];
                        Vector4 t4D = new Vector4(t.X, t.Y, t.Z, (float)w);
                        vertexStream.Write(t4D);
                    }
                    if (!options.ExcludeElements.Contains("BiTangent"))
                        vertexStream.Write(assimpMesh.BiTangents[i]);
                }
            }
            vertexStream.Dispose();

        }

        void WriteIndices(Mesh assimpMesh, ModelData.MeshPart meshPart, ModelData.IndexBuffer indexBuffer)
        {
            // Write all indices
            var indices = assimpMesh.GetIntIndices();
            indexBuffer.Count = indices.Length;
            meshPart.IndexBufferRange.Count = indices.Length;
            if (meshPart.VertexBufferRange.Count < 65536)
            {
                // Write only short indices if count is less than the size of a short
                indexBuffer.Buffer = new byte[indices.Length * 2];
                using (var indexStream = DataStream.Create(indexBuffer.Buffer, true, true))
                    foreach (int index in indices) indexStream.Write((ushort)index);
            }
            else
            {
                // Otherwise, use full 32-bit precision to store indices
                indexBuffer.Buffer = new byte[indices.Length * 4];
                using (var indexStream = DataStream.Create(indexBuffer.Buffer, true, true))
                    indexStream.WriteRange(indices);
            }

            // Update the MaximumBufferSizeInBytes needed to load this model
            if (indexBuffer.Buffer.Length > model.MaximumBufferSizeInBytes)
            {
                model.MaximumBufferSizeInBytes = indexBuffer.Buffer.Length;
            }
        }

    }
}
