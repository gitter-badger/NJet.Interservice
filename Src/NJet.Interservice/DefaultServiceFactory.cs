﻿#region License
// Copyright (c) 2016 Rajeswara-Rao-Jinaga
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

namespace NJet.Interservice
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class DefaultServiceFactory : IServiceFactory
    {
        private Dictionary<Type, Type> _services;

        public SubcontractFactory Subcontract
        {
            get;set;
        }

        public DefaultServiceFactory(string[] namespaces, Assembly[] aseemblies)
        {
            _services = new Dictionary<Type, Type>();

            if (aseemblies == null || aseemblies.Length == 0)
                return;

            BootstrapHelper.Bootstrap(namespaces,aseemblies, _services);
        }

        public T Create<T>() where T : class
        {
            Type interfaceType = typeof(T);
            Type serviceType = _services?[interfaceType];

            if (serviceType != null)
            {
                if (!InternalServiceObjectFactory.Contains(interfaceType))
                    InternalServiceObjectFactory.AddOrReplace<T>(interfaceType, serviceType, Subcontract);

                return InternalServiceObjectFactory.Get<T>();
            }

            return default(T);
        }

        public IBasicService Add<T>(Type service) where T : class
        {
            _services.Add(typeof(T), service);
            return this;
        }

        public IBasicService Replace<T>(Type service) where T : class
        {
            _services[typeof(T)] = service;
            return this;
        }

    }
}
