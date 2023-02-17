using HandyControl.Tools.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using WireDev.Erp.V1.Models.Statistics;
using static WireDev.Erp.V1.Client.Windows.Classes.CommandParameters;
using System;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class DayStatViewModel : TimeStatsViewModel
    {
        protected new List<DayStats>? _statsList = null;
        public new List<DayStats>? StatsList
        {
            get => _statsList;
            set
            {
                if (_statsList != value)
                {
                    _statsList = value;
                    base.OnPropertyChanged(nameof(StatsList));
                }
            }
        }

        public override DayStats? SelectedTime => StatsList != null && SelectedTimeStatIndex < StatsList.Count ? StatsList[SelectedTimeStatIndex] : null;

        private bool CanGetStats = true;
        public override ICommand GetStatDataCommand => new RelayCommand(param => SetStatDataAsync(param as TimeStatQuery).ConfigureAwait(true), param => CanGetStats);
        private protected override CancellationTokenSource? LoadingStatCts { get; set; } = null;

        protected override async Task<bool> SetStatDataAsync(TimeStatQuery time)
        {
            CanGetStats = false;
            StatsList ??= new();

            Task _task = new(async () =>
            {
                if (await GetStatDataAsync(time, LoadingStatCts.Token) is DayStats newData)
                {
                    LoadingStatCts.Token.ThrowIfCancellationRequested();
                    DayStats? existing = StatsList.Find(x => x.Date == newData.Date);
                    if (existing == null)
                    {
                        StatsList.Add(newData);
                    }
                    else
                    {
                        StatsList[StatsList.IndexOf(existing)] = newData;
                    }
                }
                else
                {
                    throw new ArgumentNullException();
                }
            });

            using (LoadingStatCts = new())
            {
                await HandleStatDataAsync(_task);
            }

            CanGetStats = true;
            return true;
        }
    }
}