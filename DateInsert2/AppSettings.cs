using DateInsert2.Properties;
using P3tr0viCh.Utils.Converters;
using P3tr0viCh.Utils.Settings;
using System.ComponentModel;
using System.Windows.Forms;

namespace DateInsert2
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal class AppSettings : SettingsBase<AppSettings>
    {
        [DisplayName("Сочетание клавиш")]
        public Keys HotKey { get; set; } = Keys.Shift | Keys.Control | Keys.D;

        [DisplayName("Формат даты")]
        [Description("По умолчанию: yyyy-MM-dd")]
        public string FormatDate { get; set; } = Resources.DefaultFormatDate;
    }
}