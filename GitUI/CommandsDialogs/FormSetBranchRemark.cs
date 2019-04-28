using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitUI.CommandsDialogs
{
    public partial class FormSetBranchRemark : Form
    {
        private IRemarkDataProvider _remarkDataProvider;
        private string _branchName;
        private TreeView _parentTreeView;

        public FormSetBranchRemark(object sender, string branchName)
        {
            InitializeComponent();
            _parentTreeView = sender as TreeView;
            _remarkDataProvider = new RemarkDataProvider();
            _branchName = branchName;
            lbBranchName.Text = branchName;
            tbRemark.Text = _remarkDataProvider.GetBranchRemark(_branchName);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbRemark.Clear();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _remarkDataProvider.UpdateBranch(_branchName, tbRemark.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
