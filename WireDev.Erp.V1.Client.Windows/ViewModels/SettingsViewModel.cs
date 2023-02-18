using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";
        }

        private List<Guid>? _profiles;
        public List<Guid>? Profiles
        {
            get => _profiles;
            set
            {
                if (_profiles != value)
                {
                    _profiles = value;
                    base.OnPropertyChanged(nameof(Profiles));
                }
            }
        }

        private Guid? _selectedProfile;
        public Guid? SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (_selectedProfile != value)
                {
                    _selectedProfile = value;
                    base.OnPropertyChanged(nameof(SelectedProfile));
                }
            }
        }

        private uint _nextProductNumber;
        public uint NextProductNumber
        {
            get => _nextProductNumber;
            set
            {
                if (_nextProductNumber != value)
                {
                    _nextProductNumber = value;
                    base.OnPropertyChanged(nameof(NextProductNumber));
                }
            }
        }

        private int _nextGroupNumber;
        public int NextGroupNumber
        {
            get => _nextGroupNumber;
            set
            {
                if (_nextGroupNumber != value)
                {
                    _nextGroupNumber = value;
                    base.OnPropertyChanged(nameof(NextGroupNumber));
                }
            }
        }


        private async Task ChangeProfile(Guid profile)
        {

        }
    }
}
