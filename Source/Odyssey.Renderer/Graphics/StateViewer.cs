using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Graphics.Organization;
using Odyssey.Graphics.Shaders;
using Odyssey.Organization.Commands;

namespace Odyssey.Graphics
{
    public class StateViewer
    {
        private readonly IServiceRegistry services;
        private readonly DirectXDevice device;
        private readonly LinkedList<Command> sourceCommands;
        private readonly LinkedList<Command> resultCommands;
        private PreferredRasterizerState rasterizerState;
        private PreferredBlendState blendState;
        private PreferredDepthStencilState depthStencilState;
        private LinkedListNode<Command> cursor;
        private LinkedListNode<Command> resultCursor;

        public StateViewer(IServiceRegistry services, IEnumerable<Command> list)
        {
            Contract.Requires<ArgumentNullException>(list != null, "List cannot be null.");
            this.services = services;
            device = services.GetService<IGraphicsDeviceService>().DirectXDevice;
            resultCommands = new LinkedList<Command>();
            sourceCommands = new LinkedList<Command>(list);
        }

        void AddNewCommand(Command command)
        {
            resultCursor=resultCommands.AddLast(command);
        }

        void SaveState(Technique technique)
        {
            PreferredRasterizerState preferredRasterizerState = technique.Mapping.Key.RasterizerState;
            PreferredBlendState preferredBlendState = technique.Mapping.Key.BlendState;
            PreferredDepthStencilState preferredDepthStencilState = technique.Mapping.Key.DepthStencilState;

            if (CheckRasterizerState(technique))
                AddNewCommand(new RasterizerStateChangeCommand(services, device.RasterizerStates[preferredRasterizerState.ToString()]));

            if (CheckBlendState(technique))
                AddNewCommand(new BlendStateChangeCommand(services, device.BlendStates[preferredBlendState.ToString()]));

            if (CheckDepthStencilState(technique))
                AddNewCommand(new DepthStencilStateChangeCommand(services, device.DepthStencilStates[preferredDepthStencilState.ToString()]));

            rasterizerState = preferredRasterizerState;
            blendState = preferredBlendState;
            depthStencilState = preferredDepthStencilState;
        }

        void CheckState(Command command)
        {
            var cRender = command as ITechniqueRenderCommand;
            if (cRender == null)
            {
                AddNewCommand(command);
                return;
            }

            Technique technique = cRender.Technique;

            SaveState(technique);
            AddNewCommand(command);
        }


        public bool CheckBlendState(Technique technique)
        {
            return technique.Mapping.Key.BlendState != blendState;
        }

        public bool CheckRasterizerState(Technique technique)
        {
            return technique.Mapping.Key.RasterizerState != rasterizerState;
        }

        public bool CheckDepthStencilState(Technique technique)
        {
            return technique.Mapping.Key.DepthStencilState != depthStencilState;
        }

        public LinkedList<Command> Analyze()
        {
            if (!sourceCommands.Any())
                return null;

            cursor = sourceCommands.First;

            while (cursor != null)
            {
                CheckState(cursor.Value);
                cursor = cursor.Next;
            }

            if (services.GetService<IDirectXDeviceSettings>().IsStereo)
            {
                resultCommands.AddFirst(new AlternateStereoRenderingCommand(services));
                resultCommands.AddLast(new AlternateStereoRenderingCommand(services));
                var leftEyeCommands = new LinkedList<Command>(from c in resultCommands select c);

                SaveState(((ITechniqueRenderCommand)sourceCommands.FirstOrDefault(c => c is ITechniqueRenderCommand)).Technique);
                foreach (var command in leftEyeCommands)
                    resultCommands.AddLast(command);
                resultCommands.AddLast(new AlternateStereoRenderingCommand(services));
            }
            // Go back to first ITechniqueRenderCommand
            var firstRenderCommand = (ITechniqueRenderCommand)sourceCommands.FirstOrDefault(c=> c is ITechniqueRenderCommand);
            if (firstRenderCommand!=null)
                SaveState(firstRenderCommand.Technique);
            return resultCommands;
        }
    }
}
