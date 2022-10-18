namespace Baosight.FDAA.PackageDiagnosis.Model.Enums
{
    public enum ModuleType
    {
        mtUDPInteger = 1,
        mtUDPReal = 2,
        mtUDPUserDefined = 3,
        mtUDPMulticast = 4,
        mtOPCClient = 21,
        mtDPLiteInteger = 41,
        mtDPLiteReal = 42,
        mtDPLiteDoubleInteger = 43,
        mtRFM5565 = 61,
        mtS7TCP300 = 81,
        mtS7TCP400DB = 82,
        mtS7TCP400NoneDB = 83,
        mtS7TCP = 84,
        mtS7DPRequest = 91,
        mtS7PNRequest = 92,
        mtArti3 = 96,
        mtMelsecQ = 101,
        mtCP3550 = 110,
        mtToshibaV3000 = 120,
        mtToshibaNV = 121,
        mtHitachiR700 = 130,
        mtHitachiR900 = 131,
        mtNisdas = 140,
        mtFDAA = 150,
        mtVirtualforLua = 160,
        mtVirtualforLuaTS = 161,
        mtPlayback = 241,
        mtVideoCapture = 255,
        mtTsFAU = 300,
        mtTsOPC = 301,
        mtTsUDP = 302,
        mtSme = 500
    }
}