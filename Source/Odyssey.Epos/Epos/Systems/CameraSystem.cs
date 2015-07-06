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

using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Epos.Systems
{
    public class PerspectiveCameraSystem : UpdateableSystemBase, ICameraService
    {
        private readonly List<ICamera> cameras;

        public PerspectiveCameraSystem()
            : base(Selector.All(typeof (PositionComponent), typeof (OrientationComponent), typeof (CameraComponent)))
        {
            cameras = new List<ICamera>();
        }

        public override void Start()
        {
            Subscribe<EntityChangeMessage>(ReceiveEntityChangeMessage);
            Services.AddService(typeof(ICameraService), this);

            var cameraEntity = Entities.FirstOrDefault();
            if (cameraEntity != null)
                MainCamera = cameraEntity.GetComponent<CameraComponent>();
        }

        public override void Stop()
        {
            Unsubscribe<EntityChangeMessage>();
        }
        
        void ReceiveEntityChangeMessage()
        {
            var mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
            var entity = mEntity.Source;
            var camera = entity.GetComponent<CameraComponent>();

            if (mEntity.Action == UpdateType.Add)
            {
                ResetCamera(entity);
                cameras.Add(camera);
                if (MainCamera == null)
                    MainCamera = camera;

            }
            else if (mEntity.Action == UpdateType.Remove)
            {
                cameras.Remove(camera);
                if (MainCamera == camera)
                    MainCamera = null;
            }
        }

        public override void Process(ITimeService time)
        {
            foreach (Entity entity in Entities)
            {
                var cCamera = entity.GetComponent<CameraComponent>();
                if (cCamera.Changed)
                {
                    var cPosition = entity.GetComponent<PositionComponent>();
                    var cRotation = entity.GetComponent<OrientationComponent>();
                    cCamera.View = Matrix.Translation(-cPosition.Position) * Matrix.RotationQuaternion(cRotation.Orientation);
                }
            }
        }

        public override void AfterUpdate()
        {
            base.AfterUpdate();
            foreach (Entity entity in Entities)
            {
                var cCamera = entity.GetComponent<CameraComponent>();
                cCamera.Changed = false;
            }
        }

        private void ResetCamera(Entity entity)
        {
            var deviceSettings = entity.Scene.Services.GetService<IDirectXDeviceSettings>();
            float aspectRatio = deviceSettings.PreferredBackBufferWidth/(float) deviceSettings.PreferredBackBufferHeight;
            var cCamera = entity.GetComponent<CameraComponent>();
            var cPosition = entity.GetComponent<PositionComponent>();
            var cRotation = entity.GetComponent<OrientationComponent>();
            
            TargetComponent cTarget;
            cCamera.Viewport = new ViewportF(0, 0, deviceSettings.PreferredBackBufferWidth,
                deviceSettings.PreferredBackBufferHeight);
            cCamera.Projection = Matrix.PerspectiveFovRH(cCamera.FieldOfView, aspectRatio, cCamera.NearClip, cCamera.FarClip);
            entity.TryGetComponent(out cTarget);

            Vector3 pFocus = cTarget != null ? cTarget.Location : Vector3.Zero;
            cRotation.Orientation = Quaternion.LookAtRH(cPosition.Position, pFocus, cCamera.Up);
            cCamera.View = Matrix.LookAtRH(cPosition.Position, pFocus, cCamera.Up);
        }

        public ICamera MainCamera { get; private set; }
    }
}