using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Baosight.FDAA.PackageDiagnosis;
using Baosight.FDAA.PackageDiagnosis.BLL.Packages;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using PMSServiceStatus.Properties;

namespace PMSServiceStatus
{
    /// <summary>
    ///     查看全部功能模块安装授权信息Form
    /// </summary>
    public partial class InstallationLicenseFrm : Form
    {
        #region Constructor

        public InstallationLicenseFrm()
        {
            InitializeComponent();

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersHeight = 28;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            SetLanguage();
        }

        #endregion

        #region Event

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     禁用选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void InstallationLicenseFrm_Load(object sender, EventArgs e)
        {
            FillDataGridView();
        }

        #region Field

        #endregion

        private readonly bool language = ServerConfig.getInstance().Language;

        #endregion

        #region Method

        /// <summary>
        ///     设置语言显示
        /// </summary>
        private void SetLanguage()
        {
            Text = language ? "View installation and license information" : "查看安装授权信息";
            ColumnPackage.HeaderText = Constant.ColumnPackage;
            ColumnInstallation.HeaderText = Constant.ColumnInstallation;
            ColumnLicense.HeaderText = Constant.ColumnLicense;
            btnClose.Text = language ? "Close" : "关闭";
        }

        /// <summary>
        ///     用所有功能模块的安装授权信息填充DataGridView
        /// </summary>
        private void FillDataGridView()
        {
            var packagesInfo = GetPackagesInstallationAndLicenseInfo();
            foreach (var packageInfo in packagesInfo)
            {
                Bitmap installationBitmap;
                switch (packageInfo.Item2)
                {
                    case InstallationInfo.Yes:
                        installationBitmap = Resources.tick_box_16px;
                        break;
                    case InstallationInfo.No:
                        installationBitmap = Resources.close_window_16px;
                        break;
                    case InstallationInfo.Null:
                        installationBitmap = Resources.circle_16px;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Bitmap licenseBitmap;
                switch (packageInfo.Item3)
                {
                    case LicenseInfo.Yes:
                        licenseBitmap = Resources.tick_box_16px;
                        break;
                    case LicenseInfo.No:
                        licenseBitmap = Resources.close_window_16px;
                        break;
                    case LicenseInfo.Null:
                        licenseBitmap = Resources.circle_16px;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                dataGridView1.Rows.Add(packageInfo.Item1, installationBitmap, licenseBitmap);
            }
        }

        /// <summary>
        ///     获取所有功能模块安装授权信息
        /// </summary>
        /// <returns>item1功能包名称，item2安装信息，item3授权信息</returns>
        private List<Tuple<string, InstallationInfo, LicenseInfo>> GetPackagesInstallationAndLicenseInfo()
        {
            var packagesInfo = new List<Tuple<string, InstallationInfo, LicenseInfo>>();
            var packageManager = new PackageManager();

            var taskList = new List<Task>();

            foreach (var package in packageManager.Packages)
                taskList.Add(Task.Run(() =>
                {
                    InstallationInfo installResult;

                    var installablePackage = package as IInstallation;
                    if (installablePackage != null)
                    {
                        installablePackage.CheckInstallation();
                        installResult = installablePackage.IsInstalled ? InstallationInfo.Yes : InstallationInfo.No;
                    }
                    else
                    {
                        installResult = InstallationInfo.Yes;
                    }

                    LicenseInfo licenseResult;

                    var licensablePackage = package as ILicense;
                    if (licensablePackage != null)
                    {
                        licensablePackage.CheckLicense();
                        licenseResult = licensablePackage.IsLicensed ? LicenseInfo.Yes : LicenseInfo.No;
                    }
                    else
                    {
                        licenseResult = LicenseInfo.Yes;
                    }

                    return new Tuple<string, InstallationInfo, LicenseInfo>(package.PackageName, installResult,
                        licenseResult);
                }));
            Task.WaitAll(taskList.ToArray());

            foreach (var task in taskList)
            {
                var result = ((Task<Tuple<string, InstallationInfo, LicenseInfo>>) task).Result;
                packagesInfo.Add(new Tuple<string, InstallationInfo, LicenseInfo>(result.Item1, result.Item2,
                    result.Item3));
            }

            return packagesInfo;
        }

        #endregion
    }
}