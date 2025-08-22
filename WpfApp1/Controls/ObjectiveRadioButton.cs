using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1.Controls
{
    /// <summary>
    /// 存储单个物镜按钮的配置数据
    /// </summary>
    public class ObjectiveRadioButtonModel
    {
        public string TopLabel { get; set; } = string.Empty;
        public string BottomLabel { get; set; } = string.Empty;
        public bool ClickedEnable { get; set; } = true;
        public bool IsChecked { get; set; } = false;
    }

    /// <summary>
    /// 配置管理助手，管理配置的加载、保存和更新
    /// </summary>
    public static class ObjectiveRadioButtonModelHelper
    {
        private const string DefaultFileName = "objectiveRadioButtonSettings.json";
        private const int DefaultButtonCount = 6;

        // 将配置列表序列化为JSON保存到文件
        public static void SaveSettings(this List<ObjectiveRadioButtonModel> models)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(models, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultFileName);
                File.WriteAllText(path, jsonString);
            }
            catch (Exception ex)
            {
                // 这里可以添加日志记录或其他错误处理
                throw new Exception($"保存设置时发生错误: {ex.Message}");
            }
        }

        // 加载所有RadioButton的设置
        public static List<ObjectiveRadioButtonModel> LoadSettings()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultFileName);
                if (!File.Exists(path)) return CreateDefaultSettings();
                string jsonString = File.ReadAllText(path);

                var models = JsonSerializer.Deserialize<List<ObjectiveRadioButtonModel>>(jsonString);
                return models ?? CreateDefaultSettings();
            }
            catch (Exception ex)
            {
                // 这里可以添加日志记录或其他错误处理
                throw new Exception($"加载设置时发生错误: {ex.Message}");
            }
        }

        // 创建默认设置
        private static List<ObjectiveRadioButtonModel> CreateDefaultSettings()
        {
            var defaultModels = new List<ObjectiveRadioButtonModel>
        {
            new()
            {
                BottomLabel = "/0.13",
                TopLabel = "4X",
                IsChecked = true,
                ClickedEnable = true,
            },
            new()
            {
                BottomLabel = "/0.13",
                TopLabel = "10X",
                IsChecked = false,
                ClickedEnable = false,
            },
            new()
            {
                BottomLabel = "/0.45",
                TopLabel = "20X",
                IsChecked = false,
                ClickedEnable = false,
            },
            new()
            {
                BottomLabel = "/0.65",
                TopLabel = "40X",
                IsChecked = false,
                ClickedEnable = false,
            },
            new()
            {
                BottomLabel = "/0.65",
                TopLabel = "60X",
                IsChecked = false,
                ClickedEnable = false,
            },
            new()
            {
                BottomLabel = "/1.10",
                TopLabel = "100X",
                IsChecked = false,
                ClickedEnable = false,
            }
        };
            return defaultModels;
        }

        // 更新单个RadioButton的设置
        public static void UpdateSingleButtonSetting(int index, ObjectiveRadioButtonModel model)
        {
            var settings = LoadSettings();
            if (index < 0 || index >= settings.Count) return;

            if (model.IsChecked)
            {
                for (int i = 0; i < DefaultButtonCount; i++)
                {
                    settings[i].IsChecked = false;
                }
            }

            settings[index] = model;
            SaveSettings(settings);
        }

        // 应用设置到RadioButton
        public static void ApplySettings(ObjectiveRadioButton button, ObjectiveRadioButtonModel model)
        {
            button.TopLabel = model.TopLabel;
            button.BottomLabel = model.BottomLabel;
            button.ClickedEnable = model.ClickedEnable;
            button.IsChecked = model.IsChecked;
        }

        // 从RadioButton获取当前设置
        public static ObjectiveRadioButtonModel GetCurrentSettings(ObjectiveRadioButton button)
        {
            return new ObjectiveRadioButtonModel
            {
                TopLabel = button.TopLabel,
                BottomLabel = button.BottomLabel,
                ClickedEnable = button.ClickedEnable,
                IsChecked = button.IsChecked ?? false
            };
        }
    }

    public class ObjectiveRadioButton : RadioButton
    {
        private ContextMenu? _contextMenu;

        public ObjectiveRadioButton()
        {
            InitializeContextMenu();

            this.IsChecked = false;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "IsChecked")
                ObjectiveRadioButtonModelHelper.UpdateSingleButtonSetting(GroupIndex - 1,
                    ObjectiveRadioButtonModelHelper.GetCurrentSettings(this));
        }

        #region 原始属性

        public static readonly DependencyProperty UnCheckImageProperty = DependencyProperty.Register(
            nameof(UnCheckImage), typeof(ImageSource), typeof(ObjectiveRadioButton), new PropertyMetadata(default(ImageSource)));

        public ImageSource UnCheckImage
        {
            get => (ImageSource)GetValue(UnCheckImageProperty);
            set => SetValue(UnCheckImageProperty, value);
        }

        public static readonly DependencyProperty CheckImageProperty = DependencyProperty.Register(
            nameof(CheckImage), typeof(ImageSource), typeof(ObjectiveRadioButton), new PropertyMetadata(default(ImageSource)));

        public ImageSource CheckImage
        {
            get => (ImageSource)GetValue(CheckImageProperty);
            set => SetValue(CheckImageProperty, value);
        }

        public static readonly DependencyProperty TopLabelProperty = DependencyProperty.Register(
            nameof(TopLabel), typeof(string), typeof(ObjectiveRadioButton), new PropertyMetadata(default(string)));

        public string TopLabel
        {
            get => (string)GetValue(TopLabelProperty);
            set => SetValue(TopLabelProperty, value);
        }

        public static readonly DependencyProperty BottomLabelProperty = DependencyProperty.Register(
            nameof(BottomLabel), typeof(string), typeof(ObjectiveRadioButton), new PropertyMetadata(default(string)));

        public string BottomLabel
        {
            get => (string)GetValue(BottomLabelProperty);
            set => SetValue(BottomLabelProperty, value);
        }

        public int GroupIndex
        {
            get => (int)GetValue(GroupIndexProperty);
            set => SetValue(GroupIndexProperty, value);
        }

        public static readonly DependencyProperty GroupIndexProperty =
            DependencyProperty.Register(
                nameof(GroupIndex),
                typeof(int),
                typeof(ObjectiveRadioButton),
        new PropertyMetadata(0));

        public bool ClickedEnable
        {
            get => (bool)GetValue(ClickedEnableProperty);
            set => SetValue(ClickedEnableProperty, value);
        }

        public static readonly DependencyProperty ClickedEnableProperty =
            DependencyProperty.Register(
                nameof(ClickedEnable),
                typeof(bool),
                typeof(ObjectiveRadioButton),
                new PropertyMetadata(true, OnClickedEnableChanged));

        private static void OnClickedEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ObjectiveRadioButton bt) return;
            if (e.NewValue is not bool v) return;

            if (v)
            {
                bt.Opacity = 1;
                bt.PreviewMouseLeftButtonDown -= Bt_PreviewMouseLeftButtonDown;
            }
            else
            {
                bt.Opacity = 0.2;
                bt.PreviewMouseLeftButtonDown += Bt_PreviewMouseLeftButtonDown;
            }
        }

        private static void Bt_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region 右键菜单相关

        private void InitializeContextMenu()
        {
            _contextMenu = new ContextMenu();

            // 设置顶部标签菜单项
            var setTopLabelMenuItem = new MenuItem { Header = "设置顶部标签" };
            setTopLabelMenuItem.Click += SetTopLabelMenuItem_Click;

            // 设置底部标签菜单项
            var setBottomLabelMenuItem = new MenuItem { Header = "设置底部标签" };
            setBottomLabelMenuItem.Click += SetBottomLabelMenuItem_Click;

            // 设置是否可选中菜单项
            var toggleIsEnabledMenuItem = new MenuItem { Header = "启用/禁用" };
            toggleIsEnabledMenuItem.Click += ToggleIsEnabledMenuItem_Click;

            _contextMenu.Items.Add(setTopLabelMenuItem);
            _contextMenu.Items.Add(setBottomLabelMenuItem);
            _contextMenu.Items.Add(new Separator());
            _contextMenu.Items.Add(toggleIsEnabledMenuItem);

            this.ContextMenu = _contextMenu;
        }

        private void SetTopLabelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个简单的输入对话框
            var dialog = new Lift.UI.Controls.Window
            {
                Title = "设置顶部标签",
                Width = 300,
                Height = 170,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            var textBox = new TextBox { Margin = new Thickness(0, 5, 0, 5), Text = TopLabel };
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            var okButton = new Button { Content = "确定", Width = 60, Margin = new Thickness(5) };
            var cancelButton = new Button { Content = "取消", Width = 60, Margin = new Thickness(5) };

            okButton.Click += (_, _) =>
            {
                TopLabel = textBox.Text;
                dialog.Close();
            };
            cancelButton.Click += (_, _) => dialog.Close();

            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(new TextBlock { Text = "请输入顶部标签：" });
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(buttonPanel);

            dialog.Content = stackPanel;
            dialog.ShowDialog();

            ObjectiveRadioButtonModelHelper.UpdateSingleButtonSetting(GroupIndex - 1,
                ObjectiveRadioButtonModelHelper.GetCurrentSettings(this));
        }

        private void SetBottomLabelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个简单的输入对话框
            var dialog = new Lift.UI.Controls.Window
            {
                Title = "设置底部标签",
                Width = 300,
                Height = 170,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            var textBox = new TextBox { Margin = new Thickness(0, 5, 0, 5), Text = BottomLabel };
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            var okButton = new Button { Content = "确定", Width = 60, Margin = new Thickness(5) };
            var cancelButton = new Button { Content = "取消", Width = 60, Margin = new Thickness(5) };

            okButton.Click += (_, _) =>
            {
                BottomLabel = textBox.Text;
                dialog.Close();
            };
            cancelButton.Click += (_, _) => dialog.Close();

            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(new TextBlock { Text = "请输入底部标签：" });
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(buttonPanel);

            dialog.Content = stackPanel;
            dialog.ShowDialog();

            ObjectiveRadioButtonModelHelper.UpdateSingleButtonSetting(GroupIndex - 1,
                ObjectiveRadioButtonModelHelper.GetCurrentSettings(this));
        }

        private void ToggleIsEnabledMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClickedEnable = !ClickedEnable;

            ObjectiveRadioButtonModelHelper.UpdateSingleButtonSetting(GroupIndex - 1,
                ObjectiveRadioButtonModelHelper.GetCurrentSettings(this));
        }



        #endregion
    }
}
