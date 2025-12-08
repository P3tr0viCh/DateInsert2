using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;

namespace DateInsert2
{
    public partial class Main
    {
        public const int HOTKEY_ID = 0x1;

        private bool AbnormalExit
        {
            get => Tag != null && (bool)Tag;
            set => Tag = value;
        }

        private bool CreateProgramDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }

            Directory.CreateDirectory(path);

            return true;
        }

        private bool SetProgramDirectory()
        {
            try
            {
                var programDataDirectory =
#if DEBUG
                    Files.ExecutableDirectory();
#else
                    Files.AppDataLocalDirectory();
#endif
                var programDirectoryCreated = CreateProgramDirectory(programDataDirectory);

                AppSettings.Directory = programDataDirectory;

                DebugWrite.Line($"PathDataDirectory: {programDataDirectory}");
                DebugWrite.Line($"PathAppDirectory: {Application.StartupPath}");

                DebugWrite.Line($"PathSettings: {AppSettings.FilePath}");

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                AbnormalExit = true;

                Application.Exit();

                return false;
            }
        }

        private bool RegisterHotKey() => HotKey.RegisterHotKey(Handle, AppSettings.Default.HotKey, HOTKEY_ID);
        private bool UnregisterHotKey() => HotKey.UnregisterHotKey(Handle, HOTKEY_ID);
    }
}