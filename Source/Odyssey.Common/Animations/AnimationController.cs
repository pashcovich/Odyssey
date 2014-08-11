using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics;
using Odyssey.Utilities.Reflection;
using Odyssey.Engine;

namespace Odyssey.Animations
{
    public class AnimationController
    {
        private readonly Dictionary<string, Animation> animations;
        private readonly Dictionary<string, ObjectWalker> walkers;

        private readonly List<Animation> playingAnimations;

        public IEnumerable<Animation> Animations { get { return animations.Values; } }

        public object Target { get; private set; }

        public bool HasAnimations { get { return animations.Count > 0; } }

        public bool IsPlaying { get; private set; }
        private TimeSpan elapsedTime;

        public AnimationController(object target)
        {
            animations = new Dictionary<string, Animation>();
            walkers = new Dictionary<string, ObjectWalker>();
            playingAnimations = new List<Animation>();
            Target = target;
        }

        [Pure]
        public bool ContainsAnimation(string animationName)
        {
            return animations.ContainsKey(animationName);
        }

        public void AddAnimation(Animation animation)
        {
            Contract.Requires<ArgumentNullException>(animation != null, "animation");
            Contract.Requires<ArgumentException>(!ContainsAnimation(animation.Name), "Cannot add unnamed animation");
            animations.Add(animation.Name, animation);
        }

        public void AddAnimations(IEnumerable<Animation> animations)
        {
            Contract.Requires<ArgumentNullException>(animations != null, "animations");
            foreach (var animation in animations)
                AddAnimation(animation);
        }

        public void Initialize()
        {
            foreach (var animation in animations.Values)
            {
                List<IAnimationCurve> curves = new List<IAnimationCurve>(animation.Curves);
                foreach (IAnimationCurve curve in curves)
                {
                    object realTarget;

                    if (string.IsNullOrEmpty(curve.TargetName))
                        realTarget = Target;
                    else
                    {
                        var resourceProvider = Target as IResourceProvider;
                        if (resourceProvider != null)
                            realTarget = resourceProvider.GetResource<IResource>(curve.TargetName);
                        else
                            throw new InvalidOperationException(string.Format("'Target' does not implement {0}", typeof (IResourceProvider)));
                    }

                    if (realTarget == null)
                        throw new InvalidOperationException("'Target' cannot be null");

                    ObjectWalker walker = new ObjectWalker(realTarget, curve.TargetProperty);

                    var requiresCaching = realTarget as IRequiresCaching;
                    if (requiresCaching != null)
                    {
                        var newCurve = requiresCaching.CacheAnimation(walker.CurrentMember.DeclaringType, walker.CurrentMember.Name, curve);
                        animation.RemoveCurve(curve.TargetProperty);
                        animation.AddCurve(newCurve);
                        walker = new ObjectWalker(requiresCaching, newCurve.TargetProperty);
                        walkers.Add(newCurve.TargetProperty, walker);
                    }
                    else
                    {
                        walkers.Add(curve.TargetProperty, walker);
                    }


                }
            }
        }

        public void Play()
        {
            IsPlaying = true;
            playingAnimations.AddRange(animations.Values);
        }

        public void Play(string animationName)
        {
            Contract.Requires<ArgumentNullException>(ContainsAnimation(animationName), "Animation not found");
            IsPlaying = true;
            playingAnimations.Add(animations[animationName]);
        }

        public void Stop()
        {
            IsPlaying = false;
            elapsedTime = default(TimeSpan);
            playingAnimations.Clear();
        }

        public void Stop(string animationName)
        {
            Contract.Requires<ArgumentNullException>(ContainsAnimation(animationName), "Animation not found");
            playingAnimations.Remove(animations[animationName]);
            if (playingAnimations.Count == 0)
            {
                IsPlaying = false;
                elapsedTime = default (TimeSpan);
            }
        }

        public void Update(ITimeService time)
        {
            var currentlyPlayingAnimations = new List<Animation>(playingAnimations);

            foreach (var animation in currentlyPlayingAnimations)
            {
                foreach (var curve in animation.Curves)
                {
                    var value = curve.Evaluate(elapsedTime, animation.Speed > 0);
                    var walker = GetWalker(curve.TargetProperty);
                    walker.WriteValue(value);
                    elapsedTime += time.ElapsedApplicationTime;
                    if (elapsedTime.TotalSeconds > animation.Duration)
                        Stop(animation.Name);
                }
            }
        }

        internal ObjectWalker GetWalker(string targetProperty)
        {
            return walkers[targetProperty];
        }

        public Animation this[string animationName]
        {
            get { return animations[animationName]; }
        }
    }
}
