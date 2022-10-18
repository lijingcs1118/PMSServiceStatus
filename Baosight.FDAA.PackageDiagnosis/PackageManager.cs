using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Baosight.FDAA.PackageDiagnosis.BLL;
using Baosight.FDAA.PackageDiagnosis.BLL.Packages;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis
{
    public class PackageManager
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public PackageManager()
        {
            try
            {
                InitAutofac();

                ProgramModule.Container.Resolve<IOConfig>().LoadIOConfig(FdaaHelper.CreateInstance().IoConfigName);
                ProgramModule.Container.Resolve<BasicDsConfig>().LoadBasicDSConfig();
                ProgramModule.Container.Resolve<AdvancedDsConfig>().LoadAdvancedDsConfig();
                ProgramModule.Container.Resolve<HdDsConfig>().LoadHdDsConfig();

                Packages = new List<BasePackage>
                {
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Pms),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Udp),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Arti3),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Opc),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.DpLite),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Rfm),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.S7Dprequest),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.S7Pnrequest),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.S7Tcp),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Virtual),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Melsec),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.CpNet),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.TcNet),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Usigma),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Nisdas),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Sse),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.VideoCapture),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Technostring),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Playback),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Fdaa),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.FdaaOnline),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.BasicDs),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.AdvancedDs),
                    ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Hd)
                    //ProgramModule.Container.ResolveKeyed<BasePackage>(Package.Mqtt)
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     所有的功能组件
        /// </summary>
        public List<BasePackage> Packages { get; private set; }

        /// <summary>
        ///     初始化Autofac
        /// </summary>
        private void InitAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ProgramModule>();

            ProgramModule.Container = builder.Build();
        }
    }
}