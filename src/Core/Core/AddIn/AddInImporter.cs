﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Agebull.Common;
using Agebull.Common.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace ZeroTeam.MessageMVC.ZeroApis
{
    /// <summary>
    /// MEF插件导入器
    /// </summary>
    internal class AddInImporter
    {
        /// <summary>
        /// 实例对象
        /// </summary>
        internal static AddInImporter Instance;

        /// <summary>
        /// 插件对象
        /// </summary>
        [ImportMany(typeof(IAutoRegister))]
        public IEnumerable<IAutoRegister> Registers { get; set; }

        /// <summary>
        /// 导入
        /// </summary>
        public static void Import()
        {
            if (Instance != null)
                return;
            Instance = new AddInImporter();
            IocHelper.ServiceCollection.AddSingleton(pro => Instance);
            CheckAddIn();
        }

        static void CheckAddIn()
        {
            if (string.IsNullOrEmpty(ZeroApplication.Config.AddInPath))
                return;

            var path = ZeroApplication.Config.AddInPath[0] == '/'
                ? ZeroApplication.Config.AddInPath
                : IOHelper.CheckPath(ZeroApplication.Config.RootPath, ZeroApplication.Config.AddInPath);
            ZeroTrace.SystemLog("AddIn(Service)", path);
            // 通过容器对象将宿主和部件组装到一起。 
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path);
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(Instance);
            foreach (var reg in Instance.Registers)
            {
                ZeroTrace.SystemLog("AddIn(Extend)", reg.GetType().Assembly.FullName);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            if (Registers == null)
                return;
            foreach (var reg in Registers)
            {
                reg.Initialize();
            }
        }

        /// <summary>
        /// 执行自动注册
        /// </summary>
        public void AutoRegist()
        {
            if (Registers == null)
                return;
            foreach (var reg in Registers)
                reg.AutoRegist();
        }

    }
}