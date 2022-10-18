using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    /// <summary>
    ///     DataGridView 扩展类
    /// </summary>
    public static class DataGridViewExtensions
    {
        /// <summary>
        ///     导出DataGridView to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileName">fileName</param>
        /// <param name="delimiter">分隔符</param>
        /// <remarks>
        ///     仅测试了 ,分隔符
        /// </remarks>
        public static void ExportRows(this DataGridView sender, string fileName, string delimiter)
        {
            var sb = new StringBuilder();

            var headers = sender.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(delimiter, headers.Select(column => column.HeaderText)));

            foreach (DataGridViewRow row in sender.Rows)
                if (!row.IsNewRow)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(delimiter, cells.Select(cell => EscapeCsvField(cell.Value.ToString()))));
                }

            File.WriteAllText(fileName, sb.ToString(), Encoding.Default);
        }

        /// <summary>
        ///     Companion for ExportRows
        /// </summary>
        /// <param name="fieldValueToEscape"></param>
        /// <returns></returns>
        private static string EscapeCsvField(string fieldValueToEscape)
        {
            //  因为我们用逗号分隔值，所以需要转义以下逗号。在字符串前或者后面加(")
            fieldValueToEscape = fieldValueToEscape.Replace("\n", "");
            if (fieldValueToEscape.Contains(","))
            {
                //  如果需要转义的字符串已经包含 ("), 把"替换成"",字符串前后加".
                if (fieldValueToEscape.Contains("\""))
                    return "\"" + fieldValueToEscape.Replace("\"", "\"\"") + "\"";
                return "\"" + fieldValueToEscape + "\"";
            }

            return fieldValueToEscape;
        }

        public static void ExpandColumns(this DataGridView sender)
        {
            foreach (DataGridViewColumn col in sender.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}