using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WireDev.Erp.V1.Client.Windows.Classes;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Statistics;
using static WireDev.Erp.V1.Client.Windows.Classes.CommandParameters;

namespace WireDev.Erp.V1.Client.Windows.ViewModels
{
    internal abstract class TimeStatsViewModel : BaseViewModel
    {
        public TimeStatsViewModel() { }

        protected List<DateTime>? _statsTimeList = null;
        public List<DateTime>? StatsTimeList
        {
            get => _statsTimeList;
            set
            {
                if (_statsTimeList != value)
                {
                    _statsTimeList = value;
                    base.OnPropertyChanged(nameof(StatsTimeList));
                }
            }
        }

        protected List<TimeStats>? _statsList = null;
        public virtual List<TimeStats>? StatsList
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

        public virtual TimeStats? SelectedTime => StatsList != null && SelectedTimeStatIndex < StatsList.Count ? StatsList[SelectedTimeStatIndex] : null;

        protected int _selectedTimeStatIndex;
        public int SelectedTimeStatIndex
        {
            get => _selectedTimeStatIndex;
            set
            {
                if (_selectedTimeStatIndex != value)
                {
                    _selectedTimeStatIndex = value;
                    base.OnPropertyChanged(nameof(SelectedTimeStatIndex));
                }
            }
        }


        private protected CancellationTokenSource? LoadingListCts = null;
        private protected abstract CancellationTokenSource? LoadingStatCts { get; set; }


        private bool CanGetStatList = true;
        public ICommand GetStatListCommand => new RelayCommand(param => SetStatsListAsync((TimeStatQuery)param).ConfigureAwait(true), param => CanGetStatList);

        public abstract ICommand GetStatDataCommand { get; }


        protected async Task<bool> SetStatsListAsync(TimeStatQuery time)
        {
            CanGetStatList = false;
            ushort year = time.Year;
            byte month = time.Month;

            string url = $"api/Stats/{year}/";
            url += month == 0 ? "list" : $"{month}/list";

            using (LoadingListCts = new())
            {
                try
                {
                    using HttpResponseMessage response = await ApiConnection.Client.GetAsync(url, LoadingListCts.Token);
                    Response? r = await response.Content.ReadFromJsonAsync<Response>();
                    LoadingListCts.Token.ThrowIfCancellationRequested();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StatsTimeList = r.Data is List<DateTime> list ? list : throw new ArgumentNullException("Response is not as expected!");
                        return true;
                    }
                    else
                    {
                        //await StatusCodeHandler(response.StatusCode, "Retrieving list of products");
                        throw new Exception("Request was not succesfull!");
                    }
                }
                catch (TaskCanceledException)
                {
                    //Message
                    return false;
                }
                catch (OperationCanceledException)
                {
                    //Message
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
                finally { CanGetStatList = true; }
            }
        }

        protected static async Task<TimeStats> GetStatDataAsync(TimeStatQuery time, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            ushort year = time.Year;
            byte month = time.Month;
            byte day = time.Day;

            string url = $"api/Stats/{year}/";
            url += month == 0 ? string.Empty : $"{month}/";
            url += day == 0 ? string.Empty : $"{day}";

            token.ThrowIfCancellationRequested();
            using HttpResponseMessage response = await ApiConnection.Client.GetAsync(url, token);
            Response? r = await response.Content.ReadFromJsonAsync<Response>();

            token.ThrowIfCancellationRequested();
            return response.IsSuccessStatusCode && (r.Data is TimeStats m) ? m : throw new ArgumentNullException("Response is not as expected!");
        }

        protected static async Task<bool> HandleStatDataAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            return true;
        }

        protected abstract Task<bool> SetStatDataAsync(TimeStatQuery time);
    }
}