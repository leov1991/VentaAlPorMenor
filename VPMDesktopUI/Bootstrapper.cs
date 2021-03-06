﻿using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VPMDesktopUI.Helpers;
using VPMDesktopUI.Library.API;
using VPMDesktopUI.Library.Helpers;
using VPMDesktopUI.Library.Models;
using VPMDesktopUI.Models;
using VPMDesktopUI.ViewModels;

namespace VPMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }


        protected override void Configure()
        {

            _container.Instance(ConfigureAutoMapper());

            _container.Instance(_container)
                .PerRequest<ISaleEndpoint, SaleEndpoint>()
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<IProductEndpoint, ProductEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IAPIHelper, APIHelper>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>();



            // Registrar todos los viewmodels
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        private static IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
