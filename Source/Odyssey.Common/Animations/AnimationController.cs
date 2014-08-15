using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Odyssey.Content;
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

        public object Target
        {
            get { return target; }
            set
            {
                if (target == value)
                    return;
                target = value;
                foreach (var kvp in walkers)
                {
                    var targetPropertyName = kvp.Key;
                    var walker = kvp.Value;
                    walker.SetTarget(target, targetPropertyName);
                }
            }
        }

        public bool HasAnimations { get { return animations.Count > 0; } }

        public bool IsPlaying { get; private set; }
        private float elapsedTime;
        private object target;

        public AnimationController(object target) : this()
        {
            this.target = target;
        }


        public AnimationController()
        {
            animations = new Dictionary<string, Animation>();
            walkers = new Dictionary<string, ObjectWalker>();
            playingAnimations = new List<Animation>();
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
                int cachedAnimations = 0;
                List<IAnimationCurve> curves = new List<IAnimationCurve>(animation.Curves);
                foreach (IAnimationCurve curve in curves)
                {
                    object realTarget;

                    if (string.IsNullOrEmpty(curve.TargetName))
                        realTarget = target;
                    else
                    {
                        var resourceProvider = target as IResourceProvider;
                        if (resourceProvider != null)
                            realTarget = resourceProvider.GetResource<IResource>(curve.TargetName);
                        else
                            throw new InvalidOperationException(string.Format("'Target' does not implement {0}", typeof (IResourceProvider)));
                    }

                    if (realTarget == null)
                        throw new InvalidOperationException("'Target' cannot be null");

                    ObjectWalker walker = new ObjectWalker(realTarget, curve.TargetProperty);

                    var requiresCaching = realTarget as IRequiresCaching;
                    string curveKey = curve.TargetProperty;
                    if (requiresCaching != null)
                    {
                        var newCurve = requiresCaching.CacheAnimation(walker.CurrentMember.DeclaringType, walker.CurrentMember.Name, curve);
                        if (newCurve != null)
                        {
                            animation.RemoveCurve(curve.TargetProperty);
                            animation.AddCurve(newCurve);
                            walker.SetTarget(requiresCaching, newCurve.TargetProperty);
                            curveKey = newCurve.TargetProperty;
                            cachedAnimations++;
                        }
                    }
                    var animatable = walker.CurrentMember.GetCustomAttribute<AnimatableAttribute>();
                    if (animatable == null)
                        throw new InvalidOperationException(string.Format("'{0}' is not marked as {1}", walker.CurrentMember.Name, typeof(AnimatableAttribute).Name));

                    walkers.Add(curveKey, walker);
                }

                if (cachedAnimations > 0 && cachedAnimations != curves.Count)
                    throw new InvalidOperationException(string.Format("Mixing cached and uncached animations is not supported"));
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
            
            var animation = animations[animationName];
            if (playingAnimations.Contains(animation))
                Stop(animationName);
            playingAnimations.Add(animation);
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            elapsedTime = 0;
            playingAnimations.Clear();
        }

        public void Stop(string animationName)
        {
            Contract.Requires<ArgumentNullException>(ContainsAnimation(animationName), "Animation not found");
            playingAnimations.Remove(animations[animationName]);
            if (playingAnimations.Count == 0)
            {
                IsPlaying = false;
                elapsedTime = 0;
            }
        }

        public void Update(ITimeService time)
        {
            var currentlyPlayingAnimations = new List<Animation>(playingAnimations);
            elapsedTime += time.FrameTime;

            foreach (var animation in currentlyPlayingAnimations)
            {
                animation.Time = animation.Speed >= 0 ? elapsedTime : animation.Duration - elapsedTime;

                foreach (var curve in animation.Curves)
                {
                    var value = curve.Evaluate(animation.Time);
                    var walker = GetWalker(curve.TargetProperty);
                    walker.WriteValue(value);
                }

                if (elapsedTime > animation.Duration)
                    Stop(animation.Name);

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
