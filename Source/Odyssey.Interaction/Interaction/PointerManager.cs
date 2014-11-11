using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Core;
using SharpDX;

namespace Odyssey.Interaction
{
    /// <summary>
    /// Provides cross-platform access to pointer events
    /// </summary>
    public class PointerManager : Component, IPointerService
    {
        //private readonly Application game; // keep a reference to game object to be able to initialize correctly
        private readonly IServiceRegistry services;
        private PointerPlatform platform; // keep a reference to pointer platform to be able to manipulate pointer

        private List<PointerPoint> pointerPoints = new List<PointerPoint>(); // the list of currently collected pointer points
        private List<PointerPoint> statePointerPoints = new List<PointerPoint>(); // the list of pointer points that will be copied to state

        private readonly object pointerPointLock = new object(); // keep a separate object for lock operations as "lock(this)" is a bad practice

        private bool enabled = true;
        private int updateOrder;

        /// <summary>
        /// Initializes a new instance of <see cref="PointerManager"/> class
        /// </summary>
        /// <param name="services">A reference to the <see cref="IServiceRegistry"/> used by this application.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="services"/> is null</exception>
        public PointerManager(IServiceRegistry services)
        {
            Contract.Requires<ArgumentNullException>(services != null, "services");
            this.services = services;
            this.services.AddService(typeof(IPointerService), this);
           
        }

        /// <inheritdoc/>
        public bool Enabled
        {
            get { return enabled; }
            private set
            {
                if (value == enabled) return;
                enabled = value;
                RaiseEvent(EnabledChanged);
            }
        }

        /// <inheritdoc/>
        public int UpdateOrder
        {
            get { return updateOrder; }
            private set
            {
                if (value == updateOrder) return;
                updateOrder = value;
                RaiseEvent(UpdateOrderChanged);
            }
        }

        /// <inheritdoc/>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <inheritdoc/>
        public PointerState GetState()
        {
            var state = new PointerState();

            GetState(state);

            return state;
        }

        /// <inheritdoc/>
        public void GetState(PointerState state)
        {
            if (state == null) throw new ArgumentNullException("state");

            state.Points.Clear();

            foreach (var point in statePointerPoints)
                state.Points.Add(point);
        }

        internal void Associate(PointerPlatform platform)
        {
            this.platform = ToDispose(platform);
        }

        /// <inheritdoc/>
        public void Update()
        {
            lock (pointerPointLock)
            {
                // swap the lists and clear the list that will be used to collect next pointer points

                var tmp = pointerPoints;
                pointerPoints = statePointerPoints;
                statePointerPoints = tmp;

                pointerPoints.Clear();
            }
        }

        /// <summary>
        /// Adds a pointer point to the raised events collection. It will be copied to pointer state at next update.
        /// </summary>
        /// <param name="point">The raised pointer event</param>
        internal void AddPointerEvent(ref PointerPoint point)
        {
            // use a simple lock at this time, to avoid excessive code complexity
            lock (pointerPointLock)
                pointerPoints.Add(point);
        }

        /// <summary>
        /// Raises a simple event in a thread-safe way due to stack-copy of delegate reference
        /// </summary>
        /// <param name="handler">The event handler that needs to be raised</param>
        private void RaiseEvent(EventHandler<EventArgs> handler)
        {
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
