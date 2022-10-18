using System.Collections.Generic;
using Autofac;
using Baosight.FDAA.PackageDiagnosis.BLL.Packages;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis
{
    public class ProgramModule : Module
    {
        public static IContainer Container { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<HdDsConfig>().AsSelf().SingleInstance();
            builder.RegisterType<IOConfig>().AsSelf().SingleInstance();
            builder.RegisterType<BasicDsConfig>().AsSelf().SingleInstance();
            builder.RegisterType<AdvancedDsConfig>().AsSelf().SingleInstance();
            builder.RegisterType<PmsPackage>().Keyed<BasePackage>(Package.Pms);
            builder.RegisterType<UdpPackage>().Keyed<BasePackage>(Package.Udp).OnActivating(e =>
            {
                e.Instance.UdpUnicastModules = e.Context.Resolve<IOConfig>().UdpUnicastModules;
                e.Instance.UdpMulticastModules = e.Context.Resolve<IOConfig>().UdpMulticastModules;
            });

            builder.RegisterType<Arti3Package>().Keyed<BasePackage>(Package.Arti3).OnActivating(e =>
                e.Instance.Arti3Modules = e.Context.Resolve<IOConfig>().Arti3Modules);
            builder.RegisterType<OpcPackage>().Keyed<BasePackage>(Package.Opc).OnActivating(e =>
                e.Instance.OpcModules = e.Context.Resolve<IOConfig>().OpcClientModules);

            builder.RegisterType<GenericPackageWithBoardCard<DPLiteModule>>().Keyed<BasePackage>(Package.DpLite)
                .OnActivating(e =>
                {
                    e.Instance.Modules = e.Context.Resolve<IOConfig>().DpLiteModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.ProfibusDPLiteInterface_NotInstalled_Error,
                        Code.ProfibusDPLiteInterface_NotLicensed_Error,
                        Code.ProfibusDPLiteInterface_CardDriver_NotInstalled_Error,
                        Code.ProfibusDPLiteInterface_Installed_Info,
                        Code.ProfibusDPLiteInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.PROFIBUSDPLITE_DRIVER_NAME;
                    e.Instance.Name = Constants.PROFIBUSDPLITE_PACKAGE_NAME;
                    e.Instance.BoardCardName = Constants.DP_CARD_DRIVER_NAME;
                });

            builder.RegisterType<GenericPackageWithBoardCard<RFM5565Module>>().Keyed<BasePackage>(Package.Rfm)
                .OnActivating(e =>
                {
                    e.Instance.Modules = e.Context.Resolve<IOConfig>().Rfm5565Modules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.RFMInterface_NotInstalled_Error,
                        Code.RFMInterface_NotLicensed_Error,
                        Code.RFMInterface_CardDriver_NotInstalled_Error,
                        Code.RFMInterface_Installed_Info,
                        Code.RFMInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.REFLECTIVEMEMORY5565DRIVER_NAME;
                    e.Instance.Name = Constants.REFLECTIVEMEMORY5565_PACKAGE_NAME;
                    e.Instance.BoardCardName = Constants.RFM_CARD_DRIVER_NAME;
                });

            builder.RegisterType<S7DpRequestPackage<S7DpRequestModule>>()
                .Keyed<BasePackage>(Package.S7Dprequest)
                .OnActivating(e =>
                {
                    e.Instance.Modules = e.Context.Resolve<IOConfig>().S7DpRequestModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.S7DPRequestInterface_NotInstalled_Error,
                        Code.S7DPRequestInterface_NotLicensed_Error,
                        Code.S7DPRequestInterface_CardDriver_NotInstalled_Error,
                        Code.S7DPRequestInterface_PartialNotReceivedData_Error,
                        Code.S7DPRequestInterface_Installed_Info,
                        Code.S7DPRequestInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.S7DPREQUEST_DRIVER_NAME;
                    e.Instance.Name = Constants.S7DPREQUEST_PACKAGE_NAME;
                    e.Instance.BoardCardName = Constants.DP_CARD_DRIVER_NAME;
                });

            builder.RegisterType<BoardCardWithDataReceivedPackage<S7PnRequestModule>>()
                .Keyed<BasePackage>(Package.S7Pnrequest)
                .OnActivating(e =>
                {
                    e.Instance.Modules = e.Context.Resolve<IOConfig>().S7PnRequestModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.S7PNRequestInterface_NotInstalled_Error,
                        Code.S7PNRequestInterface_NotLicensed_Error,
                        Code.S7PNRequestInterface_CardDriver_NotInstalled_Error,
                        Code.S7PNRequestInterface_PartialNotReceivedData_Error,
                        Code.S7PNRequestInterface_Installed_Info,
                        Code.S7PNRequestInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.S7PNREQUEST_DRIVER_NAME;
                    e.Instance.Name = Constants.S7PNREQUEST_PACKAGE_NAME;
                    e.Instance.BoardCardName = Constants.S7PNREQUEST_CARD_DRIVER_NAME;
                });

            builder.RegisterType<S7TcpPackage>().Keyed<BasePackage>(Package.S7Tcp).OnActivating(e =>
                e.Instance.S7TcpModules = e.Context.Resolve<IOConfig>().S7TcpModules);

            builder.RegisterType<VirtualPackage>().Keyed<BasePackage>(Package.Virtual).OnActivating(e =>
                e.Instance.VirtualModules = e.Context.Resolve<IOConfig>().VirtualModules);

            builder.RegisterType<GenericPackageWithFau<MELSECModule>>().Keyed<BasePackage>(Package.Melsec)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().MelsecModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.MELSECInterface_NotInstalled_Error,
                        Code.MELSECInterface_NotLicensed_Error,
                        Code.MELSECInterface_FauDeviceIpNotPingable_Error,
                        Code.MELSECInterface_NotReceivedData_Error,
                        Code.MELSECInterface_Installed_Info,
                        Code.MELSECInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.MELSEC_DRIVER_NAME;
                    e.Instance.Name = Constants.MELSEC_PACKAGE_NAME;
                });

            builder.RegisterType<GenericPackageWithFau<CPNetModule>>().Keyed<BasePackage>(Package.CpNet)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().CpNetModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.CPNetInterface_NotInstalled_Error,
                        Code.CPNetInterface_NotLicensed_Error,
                        Code.CPNetInterface_FauDeviceIpNotPingable_Error,
                        Code.CPNetInterface_NotReceivedData_Error,
                        Code.CPNetInterface_Installed_Info,
                        Code.CPNetInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.CPNET_DRIVER_NAME;
                    e.Instance.Name = Constants.CPNET_PACKAGE_NAME;
                });

            builder.RegisterType<GenericPackageWithFau<TCNetModule>>().Keyed<BasePackage>(Package.TcNet)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().TcNetModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.TCNetInterface_NotInstalled_Error,
                        Code.TCNetInterface_NotLicensed_Error,
                        Code.TCNetInterface_FauDeviceIpNotPingable_Error,
                        Code.TCNetInterface_NotReceivedData_Error,
                        Code.TCNetInterface_Installed_Info,
                        Code.TCNetInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.TCNET_DRIVER_NAME;
                    e.Instance.Name = Constants.TCNET_PACKAGE_NAME;
                });

            builder.RegisterType<GenericPackageWithFau<USigmaModule>>().Keyed<BasePackage>(Package.Usigma)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().USigmaModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.UsigmaInterface_NotInstalled_Error,
                        Code.UsigmaInterface_NotLicensed_Error,
                        Code.UsigmaInterface_FauDeviceIpNotPingable_Error,
                        Code.UsigmaInterface_NotReceivedData_Error,
                        Code.UsigmaInterface_Installed_Info,
                        Code.UsigmaInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.USIGMA_DRIVER_NAME;
                    e.Instance.Name = Constants.USIGMA_PACKAGE_NAME;
                });

            builder.RegisterType<GenericPackageWithFau<NisdasModule>>().Keyed<BasePackage>(Package.Nisdas)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().NisdasModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.NISDASInterface_NotInstalled_Error,
                        Code.NISDASInterface_NotLicensed_Error,
                        Code.NISDASInterface_FauDeviceIpNotPingable_Error,
                        Code.NISDASInterface_NotReceivedData_Error,
                        Code.NISDASInterface_Installed_Info,
                        Code.NISDASInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.NISDAS_DRIVER_NAME;
                    e.Instance.Name = Constants.NISDAS_PACKAGE_NAME;
                });

            builder.RegisterType<GenericPackageWithFau<SmeModule>>().Keyed<BasePackage>(Package.Sse)
                .OnActivating(e =>
                {
                    e.Instance.GenericWithFauModules = e.Context.Resolve<IOConfig>().SseModules;
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.SSEInterface_NotInstalled_Error,
                        Code.SSEInterface_NotLicensed_Error,
                        Code.SSEInterface_FauDeviceIpNotPingable_Error,
                        Code.SSEInterface_NotReceivedData_Error,
                        Code.SSEInterface_Installed_Info,
                        Code.SSEInterface_Licensed_Info
                    };
                    e.Instance.DriverName = Constants.SSE_DRIVER_NAME;
                    e.Instance.Name = Constants.SSE_PACKAGE_NAME;
                });

            builder.RegisterType<TechnostringPackage>().Keyed<BasePackage>(Package.Technostring).OnActivating(e =>
                e.Instance.TechnostringModules = e.Context.Resolve<IOConfig>().TechnostringModules);

            builder.RegisterType<VideoCapturePackage>().Keyed<BasePackage>(Package.VideoCapture).OnActivating(e =>
                e.Instance.VideoCaptureModules = e.Context.Resolve<IOConfig>().VideoCaptureModules);

            builder.RegisterType<PlaybackPackage>().Keyed<BasePackage>(Package.Playback).OnActivating(e =>
                e.Instance.PlaybackModules = e.Context.Resolve<IOConfig>().PlaybackModules);

            builder.RegisterType<FdaaPackage>().Keyed<BasePackage>(Package.Fdaa).OnActivating(e =>
            {
                e.Instance.FdaaModules = e.Context.Resolve<IOConfig>().FdaaModules;
                e.Instance.PmsServiceCollection = e.Context.Resolve<IOConfig>().PmsServiceCollection;
            });

            builder.RegisterType<FdaaOnlinePackage>().Keyed<BasePackage>(Package.FdaaOnline);

            builder.RegisterType<DataStorageBasePackage>().Keyed<BasePackage>(Package.BasicDs)
                .OnActivating(e =>
                {
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.BasicDataStorage_CacheDiscNotExist_Error,
                        Code.BasicDataStorage_CacheDiscOutOfSpace_Error
                    };
                    e.Instance.BufferLocation = Container.Resolve<BasicDsConfig>().Ds.BaseDirectory;
                    e.Instance.BufferSize = Container.Resolve<BasicDsConfig>().Ds.KeepMinimalSpace;
                    e.Instance.Name = Constants.BASIC_DATASTORAGE_PACKAGE_NAME;
                });

            builder.RegisterType<DataStorageBasePackage>().Keyed<BasePackage>(Package.AdvancedDs)
                .OnActivating(e =>
                {
                    e.Instance.CodeList = new List<Code>
                    {
                        Code.AdvancedDataStorage_CacheDiscNotExist_Error,
                        Code.AdvancedDataStorage_CacheDiscOutOfSpace_Error
                    };
                    e.Instance.BufferLocation = Container.Resolve<AdvancedDsConfig>().AdvancedDs.BaseDirectory;
                    e.Instance.BufferSize = Container.Resolve<AdvancedDsConfig>().AdvancedDs.KeepMinimalSpace;
                    e.Instance.Name = Constants.ADVANCED_DATASTORAGE_PACKAGE_NAME;
                });


            builder.RegisterType<HdDataStoragePackage>().Keyed<BasePackage>(Package.Hd);
            builder.RegisterType<MqttDataStoragePackage>().Keyed<BasePackage>(Package.Mqtt);
        }
    }
}