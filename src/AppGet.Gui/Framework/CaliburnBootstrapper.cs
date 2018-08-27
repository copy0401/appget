﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using Caliburn.Micro;
using DryIoc;
using NLog;

namespace AppGet.Gui.Framework
{
    public class CaliburnBootstrapper : BootstrapperBase
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(CaliburnBootstrapper));

        public CaliburnBootstrapper()
        {
            Initialize();
            LogConfigurator.EnableSentryTarget("https://79eabeab1aa84c8db73ee2675c5bce7d@sentry.io/306545");
            LogConfigurator.EnableFileTarget(LogLevel.Trace);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                DisplayRootViewFor<ShellViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
                throw;
            }
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var service = ContainerBuilder.Container.Resolve(serviceType, IfUnresolved.ReturnDefault);
            return service ?? base.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return ContainerBuilder.Container.ResolveMany(service);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.Exception);
            base.OnUnhandledException(sender, e);
        }
    }
}