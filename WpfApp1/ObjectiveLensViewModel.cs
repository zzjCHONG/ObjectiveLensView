using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace WpfApp1
{
   public partial class ObjectiveLensViewModel: ObservableObject
    {
        [ObservableProperty]
        private int _index;

        partial void OnIndexChanged(int value)
        {
            Debug.WriteLine($"IndexChanged {value}");
        }
    }
}
