using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Organization.Commands;
using Odyssey.Epos;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Organization
{
    public class PostProcessor
    {
        private readonly IServiceRegistry services;
        private readonly List<Command> commands;
        private readonly int width;
        private readonly int height;

        public PostProcessor(IServiceRegistry services, int width, int height)
        {
            this.services = services;
            commands = new List<Command>();
            this.width = width;
            this.height = height;
        }

        public void ProcessAction(PostProcessAction action, IEnumerable<Command> sceneCommands, string filter)
        {
            if (action.AssetName == Param.Engine)
            {
                switch (action.Technique)
                {
                    case Param.EngineActions.RenderSceneToTexture:
                        var filteredCommands = String.IsNullOrEmpty(filter) ? sceneCommands : FilterCommands(sceneCommands, filter);
                        var tDesc = GetTextureDescription((int) (width*action.Scale), (int) (height*action.Scale));
                        var command = new RenderSceneToTextureCommand(services, filteredCommands, tDesc);
                        commands.Add(command);
                        break;
                }
            }
        }

        public void Clear()
        {
            commands.Clear();
        }

        public void ProcessAction(PostProcessAction action, Model model, Technique technique, IEntity entity)
        {
            var tDesc = GetTextureDescription((int)(width * action.Scale), (int)(height * action.Scale));
            commands.Add(new PostProcessCommand(services, technique, model.Meshes[0],
                entity, tDesc, action.OutputRule) { Name = action.Technique });
        }

        public IEnumerable<Command> Result()
        {
            StateViewer sv = new StateViewer(services, commands);
            return sv.Analyze();
        }
            
        IEnumerable<Command> FilterCommands(IEnumerable<Command> commands, string tagFilter)
        {
            List<Command> filteredCommands =
                (from cRender in commands.OfType<RenderCommand>()
                 let filteredEntities = from e in cRender.Entities
                                        where e.ContainsTag(tagFilter)
                                        select e
                 where filteredEntities.Any()
                 let tRenderCommand = cRender.GetType()
                 select (RenderCommand)Activator.CreateInstance(tRenderCommand,
                             new object[] { services, cRender.Technique, cRender.Model, filteredEntities }))
                                .Cast<Command>().ToList();
            return filteredCommands;

        }

        internal static Texture2DDescription GetTextureDescription(int width, int height, Format format = Format.R8G8B8A8_UNorm)
        {
            var textureDesc = new Texture2DDescription()
            {
                ArraySize = 1,
                MipLevels = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = format,
                Width = width,
                Height = height,
                SampleDescription = new SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Default,
            };

            return textureDesc;
        }
    }
}
