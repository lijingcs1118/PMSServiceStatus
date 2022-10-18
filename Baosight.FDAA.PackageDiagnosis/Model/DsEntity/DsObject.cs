using System.Collections.Generic;

namespace Baosight.FDAA.PackageDiagnosis.Model.DsEntity
{
    // 子目录创建规则
    public enum SUB_DIRECTORY_TYPE
    {
        sdtNone = 0, // 不创建
        sdtHour = 1, // 按小时创建
        sdtDay = 2, // 按天创建
        sdtMonth = 3 // 按月创建
    }

    // 数据存储对象类型
    public enum DATA_STORAGE_TYPE
    {
        dsBasic = 0, // Basic data storage
        dsAdvanced = 1, // Advanced data storage, may replace basic ds in future
        dsHD = 2 // HD data storage
    }

    // 模拟量触发类型0:rising edge 1:falling edge 2:above level 3: below level
    // 开关量触发类型0:rising edge 1:falling edge 2:logical 0 3:logical 1
    public enum TRIGGER_TYPE
    {
        ttAnalogRisingEdge = 0,
        ttAnalogFallingEdgt = 1,
        ttAboveLevel = 2,
        ttBelowLevel = 3,
        ttDigitalRisingEdge = 4,
        ttDigitalFallingEdge = 5,
        dttLogical0 = 6,
        dttLogical1 = 7
    }

    public enum START_TRIGGER_AGAIN_MODE
    {
        sttamIgnoreIt = 0,
        sttamStartOverlappedFiles = 1
    }

    public class DSObject
    {
        // Post Process
        private string _commandLine = string.Empty;

        //--------------------------------------------------------
        // General properties
        //--------------------------------------------------------

        //--------------------------------------------------------
        // Cleanup Strategy properties
        //--------------------------------------------------------

        private int _maxOverlappedFiles = 2;

        private int _postTriggerTime = 10;
        private int _preTriggerTime = 10; //单位：秒

        // Signals
        private List<string> _signals = new List<string>();

        private START_TRIGGER_AGAIN_MODE _startTirggerAgainMode = START_TRIGGER_AGAIN_MODE.sttamStartOverlappedFiles;

        // Start trigger
        private string _startTriggerSignal = string.Empty;

        private TRIGGER_TYPE _startTriggerType = TRIGGER_TYPE.ttAnalogRisingEdge;

        // Stop trigger
        private string _stopTriggerSignal = string.Empty;
        private TRIGGER_TYPE _stopTriggerType = TRIGGER_TYPE.ttAnalogRisingEdge;

        private string _technoStringSignal = string.Empty;

        /// <summary>
        ///     默认构造函数
        /// </summary>
        public DSObject()
        {
            MaxRecordingTime = 180;
        }

        //---------------------------------------------------------------------
        /// <summary>
        ///     构造函数，决定属性的初始值
        /// </summary>
        //---------------------------------------------------------------------
        //  Copyright:  Shanghai Baosight Software Co., Ltd.
        //  返回值： 	 
        //  参数列表：	参数类型    					描述
        //  版本历史		
        //       1.0    2010-08-03 15:45 zhaoyan   First creation  
        //---------------------------------------------------------------------
        public DSObject(string id, DATA_STORAGE_TYPE dsType)
        {
            Id = id;
            DsType = dsType;
            if (dsType == DATA_STORAGE_TYPE.dsBasic)
                Active = true;
            else
                Active = false;
            FlushDataToDiskPeriod = 10;
            _commandLine = string.Empty;
            RunningGuarian = false;
            MaxRecordingTime = 300;
            SubDirectory = SUB_DIRECTORY_TYPE.sdtDay;
            MaximumFileNumber = 1000;
            BaseFileName = "fdaa";
            FileFormat = "PDAOldFormat";
            AddBaseFileName = true;
            AddFileNumber = true;
            BaseDirectory = "D:\\DATA";
            StarttimeStrategy = 1;
            KeepMinimalSpace = 500;
            DeleteVideoFiles = false;
        }

        public string Id { get; set; }

        public DATA_STORAGE_TYPE DsType { get; set; }

        public string DataStorageName { get; set; }

        public bool Active { get; set; }

        public int FlushDataToDiskPeriod { get; set; }

        //--------------------------------------------------------
        // File properties
        //--------------------------------------------------------
        public int MaxRecordingTime { get; set; }

        public string FileFormat { get; set; }

        public string BaseFileName { get; set; }

        public int MaximumFileNumber { get; set; }

        public int NextFileNumber { get; set; }

        public bool AddTechnoString { get; set; }

        public bool AddBaseFileName { get; set; }

        public bool AddFileNumber { get; set; }

        public bool AddDateTime { get; set; }

        public string BaseDirectory { get; set; }

        public SUB_DIRECTORY_TYPE SubDirectory { get; set; }

        public bool RestartFileNumberWhenSubdirectoryChanges { get; set; }

        public int StarttimeStrategy { get; set; }

        public bool IsCleanupStrategyActive { get; set; }

        public int KeepMinimalSpace { get; set; }

        public bool DeleteVideoFiles { get; set; }

        public List<string> Signals
        {
            get { return _signals; }
            set { _signals = value; }
        }

        public bool StartTriggerActive { get; set; }

        public int PreTriggerTime
        {
            get { return _preTriggerTime; }
            set { _preTriggerTime = value; }
        }

        public string StartTriggerSignal
        {
            get { return _startTriggerSignal; }
            set { _startTriggerSignal = value; }
        }

        public TRIGGER_TYPE StartTriggerType
        {
            get { return _startTriggerType; }
            set { _startTriggerType = value; }
        }

        public int StartTriggerLevel { get; set; }

        public START_TRIGGER_AGAIN_MODE StartTirggerAgainMode
        {
            get { return _startTirggerAgainMode; }
            set { _startTirggerAgainMode = value; }
        }

        public int MaxOverlappedFiles
        {
            get { return _maxOverlappedFiles; }
            set { _maxOverlappedFiles = value; }
        }

        public bool StopTriggerActive { get; set; }

        public int PostTriggerTime
        {
            get { return _postTriggerTime; }
            set { _postTriggerTime = value; }
        }

        public string StopTriggerSignal
        {
            get { return _stopTriggerSignal; }
            set { _stopTriggerSignal = value; }
        }

        public TRIGGER_TYPE StopTriggerType
        {
            get { return _stopTriggerType; }
            set { _stopTriggerType = value; }
        }

        public int StopTriggerLevel { get; set; }

        public string CommandLine
        {
            get { return _commandLine; }
            set { _commandLine = value; }
        }

        public bool RunningGuarian { get; set; }

        public string TechnoStringSignal
        {
            get { return _technoStringSignal; }
            set { _technoStringSignal = value; }
        }
    }
}