using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.angryDad.DI
{
    public class DiContainer 
    {

        private readonly DiContainer _parentContainer;
        private readonly Dictionary<(string, Type),DIRegistration> _registrations = new();
        private readonly HashSet<(string, Type Type)> _resolutions = new();
       
        public DiContainer(DiContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }

        public void RegisterSingleton<T>(Func<DiContainer, T> factory)
        {
            RegisterSingleton(null, factory);
        }

        public void RegisterSingleton<T>(string tag, Func<DiContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key,factory,true);
        }

        public void RegisterTrancient<T>(Func<DiContainer, T> factory)
        {
            RegisterTrancient(null, factory);
        }

        public void RegisterTrancient<T>(string tag, Func<DiContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }

        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof(T));
            if (_registrations.ContainsKey(key))
            {
                throw new Exception(
                $"Уже есть диай контйнер с тегом {key.Item1} и типом {key.Item2}");
            }

            _registrations[key] = new DIRegistration()
            {
                Instance = instance,
                IsSingleton = true

            };
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resolutions.Contains(key))
            {
                throw new Exception($"Другалёк, уже есть болда запрошеная с тегом {tag} и типом {key.Item2.FullName}");
            }

            _resolutions.Add(key);

            try
            {
                if (_registrations.TryGetValue(key, out var registration))
                {
                    if (registration.IsSingleton)
                    {
                        if (registration.Instance == null && registration.Factory != null)
                        {
                            registration.Instance = registration.Factory(this);
                        }

                        return (T)registration.Instance;
                    }

                    return (T)registration.Factory(this);
                }

                if (_parentContainer != null)
                {
                    return _parentContainer.Resolve<T>(tag);
                }
                
            }
            finally 
            {
                _resolutions.Remove(key);
            }


            throw new Exception($"ошибка барбос с тегом  {tag}  и  типом {key.Item2.FullName}");

        }
     


        private void Register<T>((string, Type) key, Func<DiContainer, T> factory, bool isSingleton  )
        {
            if (_registrations.ContainsKey(key))
            {
                throw new Exception(
                   $"Уже есть диай контйнер с тегом {key.Item1} и типом {key.Item2}" );
            }

            _registrations[key] = new DIRegistration()
            {
                Factory = c => factory(c),
                IsSingleton = isSingleton,
               
            };
        }

    }

}
