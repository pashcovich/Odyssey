#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Messages;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Organization.Commands;
using Odyssey.Logging;

#endregion

namespace Odyssey.Epos.Components
{
    public partial class PostProcessComponent : ContentComponent, ITechniqueComponent
    {
        private readonly List<PostProcessAction> actions;
        private readonly CommandManager commandManager;
        private Technique[] techniques;

        public PostProcessComponent() : base(ComponentTypeManager.GetType<PostProcessComponent>())
        {
            actions = new List<PostProcessAction>();
            commandManager = new CommandManager();
            Target = TargetOutput.Backbuffer;
        }

        public string TagFilter { get; set; }
        public TargetOutput Target { get; set; }

        public IEnumerable<PostProcessAction> Actions
        {
            get { return actions; }
            set
            {
                if (value.Any())
                {
                    actions.Clear();
                    actions.AddRange(value);
                    RaisePropertyChanged();
                }
            }
        }

        public CommandManager CommandManager
        {
            get { return commandManager; }
        }

        public override bool IsInited
        {
            get { return techniques != null && techniques.All(t => t.IsInited); }
        }

        public IEnumerable<Technique> Techniques
        {
            get { return techniques; }
        }

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; }

        public override bool Validate()
        {
            bool test = actions.Any();
            if (!test)
                LogEvent.Engine.Error("No post process actions defined in [{0}]", Name);

            foreach (
                var action in actions.Where(action => !string.Equals(action.AssetName, Param.Engine) && !Content.Contains(action.AssetName))
                )
            {
                LogEvent.Engine.Error("Asset [{0}] not found", action.AssetName);
                test = false;
            }

            return test;
        }

        public override void Initialize()
        {
            var techniquePool = DeviceService.DirectXDevice.TechniquePool;

            var filteredActions = (from a in actions
                where a.AssetName != Param.Engine
                select new {Asset = a.AssetName, Technique = a.Technique}).Distinct().ToArray();

            techniques = new Technique[filteredActions.Length];
            for (int i = 0; i < filteredActions.Length; i++)
            {
                var action = filteredActions[i];

                ShaderCollection shaderCollection = Content.Load<ShaderCollection>(action.Asset);

                var mapping = shaderCollection.Get(action.Technique);
                string techniqueKey = string.Format("{0}.{1}", action.Asset, mapping.Name);

                if (!techniquePool.ContainsTechnique(techniqueKey))
                {
                    techniques[i] = new Technique(DeviceService.DirectXDevice, techniqueKey, mapping);
                    techniquePool.RegisterTechnique(techniques[i]);
                    // Notifies other systems that a new technique is ready to be initialized
                    Messenger.Send(new ContentMessage<Technique>(Owner, action.Asset, techniques[i]));
                }
                else
                    techniques[i] = techniquePool.GetTechnique(techniqueKey);
            }

            var settings = Services.GetService<IDirectXDeviceSettings>();
            OutputWidth = settings.PreferredBackBufferWidth;
            OutputHeight = settings.PreferredBackBufferHeight;
        }
      
    }
}