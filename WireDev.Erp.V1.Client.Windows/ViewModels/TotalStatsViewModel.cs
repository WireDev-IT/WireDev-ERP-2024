using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Models.Statistics;
using static WireDev.Erp.V1.Client.Windows.Classes.CommandParameters;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal class TotalStatsViewModel : TimeStatsViewModel
    {
        protected new List<TotalStats>? _statsList = null;
        public new List<TotalStats>? StatsList
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

        public override TotalStats? SelectedTime => StatsList != null && SelectedTimeStatIndex < StatsList.Count ? StatsList[SelectedTimeStatIndex] : null;

        private bool CanGetStats = true;
        public override ICommand GetStatDataCommand => new RelayCommand(param => SetStatDataAsync(param as TimeStatQuery).ConfigureAwait(true), param => CanGetStats);
        private protected override CancellationTokenSource? LoadingStatCts { get; set; } = null;

        protected override async Task<bool> SetStatDataAsync(TimeStatQuery time)
        {
            CanGetStats = false;

            time.Day = 1;
            time.Month = 1;
            StatsList ??= new();

            Task _task = new(async () =>
            {
                if (await GetStatDataAsync(time, LoadingStatCts.Token) is TotalStats newData)
                {
                    LoadingStatCts.Token.ThrowIfCancellationRequested();
                    TotalStats? existing = StatsList.Find(x => x.Date == newData.Date);
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
                _ = await HandleStatDataAsync(_task);
            }

            CanGetStats = true;
            return true;
        }
    }
}