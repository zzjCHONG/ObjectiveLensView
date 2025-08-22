using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfApp1.Controls;

namespace WpfApp1
{
    /// <summary>
    /// ObjectiveLensView.xaml 的交互逻辑
    /// </summary>
    public partial class ObjectiveLensView : UserControl
    {
        #region Propdp

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(ObjectiveLensView),
                new PropertyMetadata(0, OnSelectedIndexChanged, CoerceSelectedIndex),
                new ValidateValueCallback(IsValidIndex));

        // 属性包装器
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static bool IsValidIndex(object value)
        {
            int index = (int)value;
            return index is >= 0 and <= 5;
        }

        private static object CoerceSelectedIndex(DependencyObject d, object baseValue)
        {
            int index = (int)baseValue;
            return index < 0 ? 0 : (index > 5 ? 5 : index);
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ObjectiveLensView view) return;

            view.UpdateSelectedButton((int)e.NewValue);
        }

        private void UpdateSelectedButton(int index)
        {
            RadioButton[] buttons = [Bt1, Bt2, Bt3, Bt4, Bt5, Bt6];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].IsChecked = (i == index);
            }
        }

        #endregion

        public ObjectiveLensView()
        {
            InitializeComponent();

            InitButtons();

            this.DataContext=new ObjectiveLensViewModel();

            var binding = new Binding
            {
                Path = new PropertyPath("Index"),
                Source = DataContext,
                Mode = BindingMode.TwoWay
            };
            SetBinding(SelectedIndexProperty, binding);
        }

        private void InitButtons()
        {
            var settings = ObjectiveRadioButtonModelHelper.LoadSettings();

            // todo 后面需要通过visualtree来定位name然后修改
            // todo 获取把这部分的修改移交到控件层去做，不过这样会影响最终的复用性
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt1, settings[0]);
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt2, settings[1]);
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt3, settings[2]);
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt4, settings[3]);
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt5, settings[4]);
            ObjectiveRadioButtonModelHelper.ApplySettings(Bt6, settings[5]);

            foreach (var button in new[] { Bt1, Bt2, Bt3, Bt4, Bt5, Bt6 })
            {
                button.Checked += (sender, args) =>
                {
                    SelectedIndex = Array.IndexOf([Bt1, Bt2, Bt3, Bt4, Bt5, Bt6], sender);
                };
            }
        }
    }
}
