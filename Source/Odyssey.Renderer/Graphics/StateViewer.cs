using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Organization;
using Odyssey.Graphics.Organization.Commands;
using Odyssey.Graphics.Shaders;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public class StateViewer
    {
        private readonly IServiceRegistry services;
        RasterizerState rasterizerState;
        BlendState blendState;
        DepthStencilState depthStencilState;
        private readonly LinkedList<Command> sourceCommands;
        private readonly LinkedList<Command> resultCommands;
        private LinkedListNode<Command> cursor;
        private LinkedListNode<Command> resultCursor;


        public StateViewer(IServiceRegistry services, IEnumerable<Command> list)
        {
            Contract.Requires<ArgumentNullException>(list != null, "List cannot be null.");
            this.services = services;
            resultCommands = new LinkedList<Command>();
            sourceCommands = new LinkedList<Command>(list);
        }

        void AddNewCommand(Command command)
        {
            resultCursor=resultCommands.AddLast(command);
        }

        void SaveState(Command command)
        {
            var cRender = command as IEffectRenderCommand;
            if (cRender == null)
            {
                AddNewCommand(command);
                return;
            }
            Effect effect = cRender.Effect;

            if (CheckRasterizerState(effect))
                AddNewCommand(new RasterizerStateChangeCommand(services, effect.PreferredRasterizerState));

            if (CheckBlendState(effect))
                AddNewCommand(new BlendStateChangeCommand(services, effect.PreferredBlendState));

            if (CheckDepthStencilState(effect))
                AddNewCommand(new DepthStencilStateChangeCommand(services, effect.PreferredDepthStencilState));

            AddNewCommand(command);

            rasterizerState = effect.PreferredRasterizerState;
            blendState = effect.PreferredBlendState;
        }

        
        public bool CheckBlendState(Effect effect)
        {
            return blendState == null || string.Equals(effect.PreferredBlendState.Name, blendState.Name);
        }

        public bool CheckRasterizerState(Effect effect)
        {
            return rasterizerState==null || string.Equals(effect.PreferredRasterizerState.Name, rasterizerState.Name);
        }

        public bool CheckDepthStencilState(Effect effect)
        {
            return depthStencilState == null || string.Equals(effect.PreferredBlendState.Name, depthStencilState.Name);
        }

        public void SetRasterizerState(RasterizerState newRasterizerState)
        {
            rasterizerState = newRasterizerState;
        }

        public void SetBlendState(BlendState newBlendState)
        {
            blendState = newBlendState;
        }

        public void SetDepthStencilState(DepthStencilState newDepthStencilState)
        {
            depthStencilState = newDepthStencilState;
        }


        public LinkedList<Command> Analyze()
        {
            if (!sourceCommands.Any())
                return null;

            cursor = sourceCommands.First;


            while (cursor.Next != null)
            {
                SaveState(cursor.Value);
                cursor = cursor.Next;
            }

            SaveState(cursor.Value);
            return resultCommands;
        }
    }
}
