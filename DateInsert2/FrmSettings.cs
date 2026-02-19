using DateInsert2.Properties;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System.Windows.Forms;

namespace DateInsert2
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner)
        {
            using (var frm = new FrmSettings())
            {
                AppSettings.Default.Save();

                if (frm.ShowDialog(owner) == DialogResult.OK)
                {
                    AppSettings.Default.Save();

                    return true;
                }
                else
                {
                    AppSettings.Default.Load();

                    return false;
                }
            }
        }

        private void FrmSettings_Load(object sender, System.EventArgs e)
        {
            propertyGrid.SelectedObject = AppSettings.Default;

            propertyGrid.ExpandAllGridItems();
        }

        private bool CheckData()
        {
            if (AppSettings.Default.FormatDate.IsEmpty())
            {
                AppSettings.Default.FormatDate = Resources.DefaultFormatDate;
            }

            if (AppSettings.Default.HotKey == Keys.None)
            {
                Msg.Error(Resources.ErrorRegisterHotKeyEmpty);
                return false;
            }

            var result = HotKey.RegisterHotKey(Handle, AppSettings.Default.HotKey, 0);

            if (result)
            {
                HotKey.UnregisterHotKey(Handle, 0);
            }
            else
            {
                Msg.Error(Resources.ErrorRegisterHotKey, HotKey.ConvertToString(AppSettings.Default.HotKey));
            }

            return result;
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (CheckData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}