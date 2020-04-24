﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public enum Lifetime
    {
        Singlelton,
        Self,
        Transient
    }

    /// <summary>
    /// 服务注册类
    /// </summary>
    public class ServiceRegistry
    {
        public Type ServiceType { get; }
        public Lifetime Lifetime { get; }
        public Func<Cat, Type[], object> Factory { get; }
        internal ServiceRegistry Next { get; set; }

        public ServiceRegistry(Type serviceType, Lifetime lifetime, Func<Cat, Type[], object> factory)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            Factory = factory;
        }

        public IEnumerable<ServiceRegistry> AsEnumerable()
        {
            var list = new List<ServiceRegistry>();
            for (var self = this; self != null; self = self.Next)
            {
                list.Add(self);
            }
            return list;
        }
    }

    /// <summary>
    /// DI容器类
    /// </summary>
    public class Cat : IServiceProvider, IDisposable
    {
        internal Cat _root;
        internal ConcurrentDictionary<Type, ServiceRegistry> _registries; //所有添加的服务注册
        private ConcurrentDictionary<ServiceRegistry, object> _services;
        private ConcurrentBag<IDisposable> _disposables;
        private volatile bool _disposed;

        public Cat()
        {
            _registries = new ConcurrentDictionary<Type, ServiceRegistry>();
            _root = this;
            _services = new ConcurrentDictionary<ServiceRegistry, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }
        internal Cat(Cat parent)
        {
            _root = parent._root;
            _registries = _root._registries;
            _services = new ConcurrentDictionary<ServiceRegistry, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="registry"></param>
        /// <returns></returns>
        public Cat Register(ServiceRegistry registry)
        {
            EnsureNotDisposed();
            if (_registries.TryGetValue(registry.ServiceType, out var existing))
            {
                _registries[registry.ServiceType] = registry;
                registry.Next = existing;
            }
            else
            {
                _registries[registry.ServiceType] = registry;
            }
            return this;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            EnsureNotDisposed();
            if (serviceType == typeof(Cat))
            {
                return this;
            }
            ServiceRegistry registry;
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = serviceType.GetGenericArguments()[0];
                if (!_registries.TryGetValue(elementType, out registry))
                {
                    return Array.CreateInstance(elementType, 0);
                }

                var registries = registry.AsEnumerable();
                var services = registries.Select(it => GetServiceCore(it, new Type[0])).ToArray();
                Array array = Array.CreateInstance(elementType, services.Length);
                services.CopyTo(array, 0);
                return array;
            }

            if (serviceType.IsGenericType && !_registries.ContainsKey(serviceType))
            {
                var definition = serviceType.GetGenericTypeDefinition();
                return _registries.TryGetValue(definition, out registry)
                    ? GetServiceCore(registry, serviceType.GetGenericArguments())
                    : null;
            }

            return _registries.TryGetValue(serviceType, out registry)
                    ? GetServiceCore(registry, new Type[0])
                    : null;
        }

        private object GetServiceCore(ServiceRegistry registry, Type[] genericArguments)
        {
            var serviceType = registry.ServiceType;
            object GetOrCreate(ConcurrentDictionary<ServiceRegistry, object> services, ConcurrentBag<IDisposable> disposables)
            {
                if (services.TryGetValue(registry, out var service))
                {
                    return service;
                }
                service = registry.Factory(this, genericArguments);
                services[registry] = service;
                var disposable = service as IDisposable;
                if (null != disposable)
                {
                    disposables.Add(disposable);
                }
                return service;
            }

            switch (registry.Lifetime)
            {
                case Lifetime.Singlelton: return GetOrCreate(_root._services, _root._disposables);
                case Lifetime.Self: return GetOrCreate(_services, _disposables);
                default:
                    {
                        var service = registry.Factory(this, genericArguments);
                        var disposable = service as IDisposable;
                        if (null != disposable)
                        {
                            _disposables.Add(disposable);
                        }
                        return service;
                    }
            }
        }

        public void Dispose()
        {
            _disposed = true;
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            while (!_disposables.IsEmpty)
            {
                _disposables.TryTake(out _);
            }
            _services.Clear();
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Cat");
            }
        }
    }
}
