using System;
using System.Linq;
using Autofac;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using com.baosight.hdsdk;
using com.baosight.hdsdk.core;
using com.baosight.hdsdk.domain.data;
using com.baosight.hdsdk.exception;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class HdManager
    {
        private readonly HDConnectionManager connectionManager;
        private readonly HDDataProvider dataProvider;
        private readonly HDDataManager hdDataManager;
        private uint tagId;

        public HdManager()
        {
            connectionManager = new HDConnectionManager();
            hdDataManager = new HDDataManager();
            dataProvider = new HDDataProvider(hdDataManager);
        }

        /// <summary>
        ///     连接HD
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>连接结果 成功为True 失败False</returns>
        public bool ConnectToHd(out string error)
        {
            var hdDsConfig = ProgramModule.Container.Resolve<HdDsConfig>();
            var server = hdDsConfig.HdObject.HDServers.FirstOrDefault();
            var ip = server.Value.IP;
            var port = Convert.ToUInt16(server.Value.Port);
            var timeOut = 2;

            var connection = new HDConnection(ip, port, "", port, timeOut);

            try
            {
                connectionManager.ConnectToServer(connection);
                error = string.Empty;
                return true;
            }
            catch (HDSdkException ex)
            {
                connectionManager.DisconnectFromServer();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        ///     根据TagName查询TagId
        /// </summary>
        /// <param name="tagName">tagName</param>
        /// <returns>tagId</returns>
        private uint QueryHdTestTagIdByName(string tagName, out string error)
        {
            try
            {
                var tagManager = hdDataManager.getTagManager();

                tagId = tagManager.getTagIDByTagName(tagName);
                error = string.Empty;
            }
            catch (Exception e)
            {
                error = e.Message;
                tagId = 0;
            }

            return tagId;
        }

        /// <summary>
        ///     是否可以查询测试点
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>是否可以查询 可以为True 不可以为False</returns>
        public bool QueryTestTagSnapShot(out string error)
        {
            error = string.Empty;

            //  测试点名字
            var hdTestTagName = Constants.HD_TEST_TAG_PREFIX + ProgramModule.Container.Resolve<IOConfig>().DbName;

            //  测试点ID
            tagId = QueryHdTestTagIdByName(hdTestTagName, out error);

            if (error != string.Empty) return false;

            try
            {
                //  查询测试点
                dataProvider.QuerySnapshotByTagID(tagId);
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        /// <summary>
        ///     检查测试点时间戳和本地时间戳间隔是否达标
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>达标返回True 否则返回False</returns>
        public bool CheckTimeStampIntervalIsMeet(out string error)
        {
            HDDataRecord record;
            try
            {
                //  查询测试点
                record = dataProvider.QuerySnapshotByTagID(tagId);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            var testTagTime = ConvertTagDateTime(record);
            var seconds = Math.Abs((testTagTime - DateTime.Now).TotalSeconds);

            //  时间戳相差大于3秒则 判定不符合要求
            if (seconds > 3)
            {
                error = string.Empty;
                return false;
            }

            error = string.Empty;
            return true;
        }

        /// <summary>
        ///     断开HD连接
        /// </summary>
        public void DisConnectHd()
        {
            connectionManager.DisconnectFromServer();
        }

        /// <summary>
        ///     通过record获取准确时间
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private static DateTime ConvertTagDateTime(HDDataRecord record)
        {
            var dateTime = Helper.GetDateTimeBySeconds(record.getSecond());
            var microSeconds = record.getMicroSecond();
            var preciseDateTime = dateTime.AddMilliseconds(microSeconds);
            return preciseDateTime;
        }
    }
}