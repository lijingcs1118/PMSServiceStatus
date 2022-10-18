namespace PMSServiceStatus
{
    public static class Constant
    {
        public static readonly string ColumnHearCode = ServerConfig.getInstance().Language ? "Code" : "诊断码";
        public static readonly string ColumnHearDetail = ServerConfig.getInstance().Language ? "Detail" : "诊断结果";
        public static readonly string ColumnHearSuggestion = ServerConfig.getInstance().Language ? "Suggestion" : "建议举措";
        public static readonly string ColumnPackage = ServerConfig.getInstance().Language ? "Package name" : "功能包名称";
        public static readonly string ColumnInstallation = ServerConfig.getInstance().Language ? "Installation" : "安装信息";
        public static readonly string ColumnLicense = ServerConfig.getInstance().Language ? "license" : "授权信息";
    }
}