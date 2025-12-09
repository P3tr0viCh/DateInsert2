using DateInsert2.Properties;
using P3tr0viCh.Utils;
using System;
using System.Windows.Forms;

namespace DateInsert2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case HotKey.WM_HOTKEY:
                    OnHotKeyPressed(m.WParam.ToInt32());
                    break;
            }
        }

        protected virtual void OnHotKeyPressed(int hotKeyId)
        {
            DebugWrite.Line($"ID = {hotKeyId}");

            InsertDate();
        }

#if !DEBUG
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;

            base.OnLoad(e);
        }
#endif

        private void BtnMenu_Click(object sender, EventArgs e)
        {
            mainMenu.ShowFromButton(btnMenu);
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private bool UserClosing = false;

        private void UserClose()
        {
            UserClosing = true;
            Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            UserClose();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Text = string.Format(Resources.TitleMain, new AssemblyDecorator().VersionString(false));

            if (!SetProgramDirectory()) return;

            AppSettings.Load();

            SettingsChanged();
        }

        private bool AppClosing(CloseReason closeReason)
        {
            if (AbnormalExit) return false;

            switch (closeReason)
            {
                case CloseReason.WindowsShutDown:
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                    break;
                default:
                    miClose.Enabled = false;

                    if (UserClosing)
                    {
                        UserClosing = false;

#if !DEBUG
                            if (!Msg.Question(Resources.QuestionCloseProgram))
                            {
                                miClose.Enabled = true;

                                return true;
                            }
#endif
                    }

                    AppSettings.Save();

                    break;
            }

            return false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = AppClosing(e.CloseReason);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey();
        }

        private void ShowSettings()
        {
            miSettings.Enabled = false;

            try
            {
                UnregisterHotKey();

                if (FrmSettings.ShowDlg(this))
                {
                    SettingsChanged();
                }
                else
                {
                    RegisterHotKey();
                }
            }
            finally
            {
                RegisterHotKey();

                miSettings.Enabled = true;
            }
        }

        private void SettingsChanged()
        {
            RegisterHotKey();
        }

        private void ShowAbout()
        {
            miAbout.Enabled = false;

            FrmAbout.Show(new FrmAbout.Options()
            {
                AppNameFontSize = 18
            });

            miAbout.Enabled = true;
        }

        private void MiSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void MiAbout_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        private void MiClose_Click(object sender, EventArgs e)
        {
            UserClose();
        }

        private bool inserting = false;

        private void InsertDate()
        {
            if (inserting) return;

            inserting = true;

            while (HotKey.IsAnyModifyers())
            {
                Application.DoEvents();
            }

            var date =
#if DEBUG
                new Random().Next().ToString();
#else
                DateTime.Now.ToString(AppSettings.Default.FormatDate);
#endif

            DebugWrite.Line(date);

            Clipboard.SetText(date);

            SendKeys.SendWait("(+){INSERT}");

            inserting = false;
        }
    }
}