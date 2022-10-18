using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Baosight.FDAA.PackageDiagnosis.BLL;
using MakarovDev.ExpandCollapsePanel;

namespace PMSServiceStatus
{
    public partial class ExportFrm : Form
    {
        #region Constructor

        public ExportFrm(IEnumerable<CostomDataGridView> dataGridViews)
        {
            overViewDataTable = new DataTable();
            overViewDataTable.Columns.Add(Constant.ColumnHearCode);
            overViewDataTable.Columns.Add(Constant.ColumnHearDetail);
            overViewDataTable.Columns.Add(Constant.ColumnHearSuggestion);

            this.dataGridViews = dataGridViews;
            
            InitializeComponent();
            txtExportPath.Text = Path.Combine(FdaaHelper.CreateInstance().DiagnosePath, "PackageDiagnosticResult.csv");
            SetLanguage();
        }

        #endregion

        #region Method

        /// <summary>
        ///     设置语言显示
        /// </summary>
        private void SetLanguage()
        {
            Text = language ? "Export Diagnostic Result" : "导出诊断结果";
            lblSaveText.Text = language ? "Exported file(.csv) : " : "导出文件（.csv）：";
            btnBrowse.Text = language ?  "Browse . . .": "浏览 . . .";
            lblPreview.Text = language ?  "Preview:": "预览：";
            btnCopy.Text = language ?  "Copy to Clipboard": "复制到剪贴板";
            btnSave.Text = language ? "Export" : "导出";
            btnClose.Text = language ?  "Close": "关闭";
        }

        #endregion

        #region Field

        private readonly IEnumerable<CostomDataGridView> dataGridViews;
        private readonly DataTable overViewDataTable;
        private readonly bool language = ServerConfig.getInstance().Language;
        private readonly string exportedSuccessfully = ServerConfig.getInstance().Language ? "Successfully exported!" : "导出成功！";

        #endregion

        #region Event

        /// <summary>
        ///     给DataGridView初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportFrm_Load(object sender, EventArgs e)
        {
            foreach (var dataGridView in dataGridViews)
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var newRow = overViewDataTable.NewRow();
                newRow[Constant.ColumnHearCode] = row.Cells[Constant.ColumnHearCode].Value.ToString().Replace("\n", "");
                newRow[Constant.ColumnHearDetail] = row.Cells[Constant.ColumnHearDetail].Value.ToString().Replace("\n", ""); ;
                newRow[Constant.ColumnHearSuggestion] = row.Cells[Constant.ColumnHearSuggestion].Value.ToString().Replace("\n", ""); ;
                overViewDataTable.Rows.Add(newRow);
            }

            overViewDataGridView.DataSource = overViewDataTable;

            overViewDataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            overViewDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            

            //  dataGridView 列宽设为百分比
            if (overViewDataGridView == null) return;

            overViewDataGridView.Columns[Constant.ColumnHearCode].FillWeight = 9;
            overViewDataGridView.Columns[Constant.ColumnHearDetail].FillWeight = 36;
            overViewDataGridView.Columns[Constant.ColumnHearSuggestion].FillWeight = 55;
        }

        /// <summary>
        ///     粘贴到剪切板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            overViewDataGridView.SelectAll();
            var dataObj = overViewDataGridView.GetClipboardContent();
            if (dataObj != null) Clipboard.SetDataObject(dataObj, true);
        }

        /// <summary>
        ///     禁用DataGridView排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void overViewDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in overViewDataGridView.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        /// <summary>
        ///     选择保存文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save as";
            saveFileDialog.InitialDirectory = FdaaHelper.CreateInstance().DiagnosePath;
            saveFileDialog.Filter = "Csv files (*.csv)|*.csv";
            saveFileDialog.FileName = "PackageDiagnosticResult.csv";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) txtExportPath.Text = saveFileDialog.FileName;
        }

        /// <summary>
        ///     导出CSV文件到指定文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (overViewDataGridView.Rows.Count > 0)
                try
                {
                    overViewDataGridView.ExportRows(txtExportPath.Text, ",");
                    MessageBox.Show(exportedSuccessfully, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
                MessageBox.Show(language ? "No Record To Export!" : "没有可以导出的数据！", "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}