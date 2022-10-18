using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    public partial class StaffListPanel : UserControl
    {
        public StaffListPanel()
        {
            InitializeComponent();
            InitText();
        }

        private void InitText()
        {
            lblProductionStaff.Text = "FDAA - Production Staff";
            lblCopyright.Text = "Copyright(C) 2003-2022 by Baosight Corporation \n All Rights Reserved";
            lblProductManager.Text = "Product Manager";
            lblProductManagerCredit.Text = "Yan Zhao";
            lblSystemDesigning.Text = "System Designing";
            lblSystemDesigningCredit.Text = "Yan Zhao, Huaibing Shen";
            lblProgramming.Text = "Programming";
            lblProgrammingCredit.Text = "Huaibing Shen, Gang Meng \n Jianwei Lu, Yan Zhao, Zhenyu Cai \n Bingbing Wang, Can Huang, Jing Li, Haotong Wei \n Mu Ma, Long Yang, Wangnan Han";
            lblQA.Text = "QA";
            lblQACredit.Text = "Weili Chen,  Anqi Xiao";
            lblArtDesigning.Text = "Art Designing";
            lblArtDesigningCredit.Text = "Yihui Wang";
            lblDocumentation.Text = "Documentation";
            lblDocumentationCredit.Text = "Yan Zhao,  Xiaoying Tang, Jianwei Lu \n Pengfei Chen, Bingbing Wang, Tongguo Tang \n Can Huang, Jing Li";
            lblTest.Text = "Test";
            lblTestCredit.Text = "Xiaoying Tang, Ling Xu, Kai Zhou";
            lblSystemSupport.Text = "System Support";
            lblSystemSupportCredit.Text = "Pengfei Chen, Bingbing Wang \n Liping Jiang, Lei Xin, Daojun Ji, Shuang Wang";
            lblSpecialThanksto.Text = "Special Thanks to";
            lblSpecialThankstoCredit.Text = "Shuogong Zhang \n Jianhu Wang, Guohua Liu \n Lei Zhang, Weidong Qian \n Guojun Chen, Dehuan Luo \n Yiping Wu, Haigang Li, Jianming Zeng \n Yuhua Wang \n Donghong Wang, Xiangming Huang \n Duodi Zhang, Mengmeng Xiao \n Donghua Gao, Bing Shen \n Jie Shen, Shanshan Cai, Lu Han \n Yu Wu, Liang Wu";
        }
    }
}
