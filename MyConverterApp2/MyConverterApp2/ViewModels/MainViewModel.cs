using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyConverterApp2.Models;
using MyConverterApp2.Services;

namespace MyConverterApp2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IRateService rateService;
        private readonly ILengthService lengthService;

        private const string PrefPinnedTzId = "PinnedTimeZoneId";
        private readonly IDispatcherTimer _timer;

        public ObservableCollection<TimeZoneItem> TimeZones { get; } = new();

        [ObservableProperty] private string? selectedTargetTzId;
        [ObservableProperty] private TimeZoneItem? selectedTimeZone;
        [ObservableProperty] private DateTime localTime;
        [ObservableProperty] private DateTime targetTime;
        [ObservableProperty] private string? pinnedTzId;
        [ObservableProperty] private bool isPinned;
        [ObservableProperty] private string? targetTzOffsetText;   // e.g., "UTC+1 (BST)"

        [ObservableProperty] private Unit? unit;

        [ObservableProperty] string? conversionResult;

        [ObservableProperty] ObservableCollection<string>? unitTypes;

        [ObservableProperty] string? selectedUnitType;

        [ObservableProperty] ObservableCollection<Currency>? currencyBaseNames;

        [ObservableProperty] ObservableCollection<string>? lengthBaseNames;

        [ObservableProperty] private string? currencyConversionSummary;

        [ObservableProperty] string? lengthConversionSummary;

        [ObservableProperty] bool isResultLabelVisible;

        [ObservableProperty] bool isLengthResultLabelVisible;

        [ObservableProperty] private bool isCurrencySummaryVisible;


        public MainViewModel(IRateService rateService, ILengthService lengthService)
        {
            this.rateService = rateService;
            this.lengthService = lengthService;
            SetCurrencyBaseNames();
            SetLengthBaseNames();
            IsResultLabelVisible = false;
            IsLengthResultLabelVisible = false;

            Unit = new Unit();
            if (Unit != null)
            {
                Unit.PropertyChanged += OnUnitPropertyChanged;
            }
            Unit.AutoConvertCallback = AutoConvertAsync;
            Unit.LengthAutoConvertCallback = LengthAutoConvertAsync;

            // 1) Build time zone list (sorted: UTC offset, then name)
            var zones = TimeZoneInfo.GetSystemTimeZones()
                .OrderBy(z => z.BaseUtcOffset)
                .ThenBy(z => z.DisplayName)
                .Select(z => new TimeZoneItem(z.Id, z.DisplayName));

            foreach (var z in zones)
                TimeZones.Add(z);

            // 2) Initialize local time
            LocalTime = DateTime.Now;

            // 3) Load pinned TZ (if any). Fall back to Europe/London, else first item.
            var pinned = Preferences.Get(PrefPinnedTzId, null);
            string fallbackId = TimeZones.FirstOrDefault(t => t.Id == "America/Chicago")?.Id
                                ?? TimeZones.FirstOrDefault()?.Id
                                ?? "UTC"; // ultra-safe fallback (shouldn't hit if list is non-empty)

            var initialId = !string.IsNullOrWhiteSpace(pinned) && TimeZones.Any(t => t.Id == pinned)
                ? pinned
                : fallbackId;

            // Keep both properties in sync up front
            SelectedTargetTzId = initialId;
            SelectedTimeZone   = TimeZones.FirstOrDefault(t => t.Id == initialId);

            // If we restored a pinned zone, mark as pinned
            IsPinned = !string.IsNullOrWhiteSpace(pinned) && pinned == initialId;
            PinnedTzId = IsPinned ? pinned : null;

            // 4) Compute initial conversion
            RecalculateTarget();

            // 5) Keep "now" fresh every 30s (or change to 60s if you prefer)
            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += (_, __) =>
            {
                LocalTime = DateTime.Now;
                RecalculateTarget();
            };
            _timer.Start();
        }

        partial void OnSelectedTargetTzIdChanged(string? oldValue, string? newValue)
        {
            // keep SelectedTimeZone in sync when SelectedTargetTzId changes (e.g., after load/pin)
            SelectedTimeZone = TimeZones.FirstOrDefault(z => z.Id == newValue);
            RecalculateTarget();
            if (IsPinned) SavePinned();
        }

        partial void OnSelectedTimeZoneChanged(TimeZoneItem? oldValue, TimeZoneItem? newValue)
        {
            // keep SelectedTargetTzId in sync when user picks from the UI
            SelectedTargetTzId = newValue?.Id;
            RecalculateTarget();
            if (IsPinned) SavePinned();
        }


        partial void OnLocalTimeChanged(DateTime oldValue, DateTime newValue) => RecalculateTarget();

        [RelayCommand]
        private void TogglePin()
        {
            if (string.IsNullOrWhiteSpace(SelectedTargetTzId)) return;

            IsPinned = !IsPinned;
            if (IsPinned)
            {
                PinnedTzId = SelectedTargetTzId;
                SavePinned();
            }
            else
            {
                Preferences.Remove(PrefPinnedTzId);
                PinnedTzId = null;
            }
        }

        [RelayCommand]
        private void UsePinned()
        {
            if (!string.IsNullOrWhiteSpace(PinnedTzId) && TimeZones.Any(t => t.Id == PinnedTzId))
                SelectedTargetTzId = PinnedTzId;
        }

        [RelayCommand]
        private void SetLocalNow()
        {
            LocalTime = DateTime.Now;
        }

        private void SavePinned() => Preferences.Set(PrefPinnedTzId, PinnedTzId);

        private void RecalculateTarget()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedTargetTzId))
                    return;

                var localTz = TimeZoneInfo.Local;
                var targetTz = TimeZoneInfo.FindSystemTimeZoneById(SelectedTargetTzId);

                // Treat LocalTime as local-zone time (not unspecified/UTC)
                var localDt = DateTime.SpecifyKind(LocalTime, DateTimeKind.Unspecified);
                var localWithKind = TimeZoneInfo.ConvertTime(localDt, localTz); // attach local zone context

                var utc = TimeZoneInfo.ConvertTimeToUtc(localWithKind, localTz);
                var target = TimeZoneInfo.ConvertTimeFromUtc(utc, targetTz);

                TargetTime = target;
                TargetTzOffsetText = FormatOffset(targetTz, target);
            }
            catch
            {
                // if a user picks an exotic/removed zone, fallback gracefully
                TargetTime = DateTime.MinValue;
                TargetTzOffsetText = "Unavailable";
            }
        }


        private static string FormatOffset(TimeZoneInfo tz, DateTime when)
        {
            var offset = tz.GetUtcOffset(when);
            var sign = offset < TimeSpan.Zero ? "-" : "+";
            offset = offset.Duration();
            var abbrev = tz.IsDaylightSavingTime(when) ? "DST" : "STD";
            return $"UTC{sign}{offset.Hours:00}:{offset.Minutes:00} ({abbrev})";
        }

        public void Dispose() => _timer?.Stop();

        partial void OnUnitChanged(Unit? oldValue, Unit? newValue)
        {
            if (oldValue != null) oldValue.PropertyChanged -= OnUnitPropertyChanged;
            if (newValue != null) newValue.PropertyChanged += OnUnitPropertyChanged;
        }

        /// <summary>
        /// Resets Currency UnitValue and Currency Pickers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Unit.UnitValue))
            {
                // Auto-reset the result when user starts a new input
                ConversionResult = string.Empty;
                IsResultLabelVisible = false;
                IsCurrencySummaryVisible = false;
                Unit.SelectedFromUnit = null;
                Unit.SelectedToUnit = null;
            }
        }

        // Currency: Automatically converts and displays currency result
        private async Task AutoConvertAsync()
        {
            if (!string.IsNullOrWhiteSpace(Unit?.SelectedFromUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.SelectedToUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.UnitValue))
            {
                await GetRatesAsync();
            }
        }

        // Length: Automatically converts and displays length result
        private async Task LengthAutoConvertAsync()
        {
            if (!string.IsNullOrWhiteSpace(Unit?.LengthSelectedFromUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.LengthSelectedToUnit) &&
                !string.IsNullOrWhiteSpace(Unit?.LengthUnitValue))
            {
                await ConvertLength();
            }
        }

        // Currency: Base Names
        private async Task SetCurrencyBaseNames()
        {
            CurrencyBaseNames = await rateService.SetBaseNames();
        }

        // Length: Base Names
        private void SetLengthBaseNames()
        {
            LengthBaseNames = lengthService.SetBaseNames();
        }

        // Utility Methods
        public string SplitBaseString(string s)
        {
            return s.Split(' ')[0];
        }

        // Currency: Clear Results
        [RelayCommand]
        public async Task ClearResultAsync()
        {
            ConversionResult = "";
            Unit.UnitValue = "";
            Unit.SelectedFromUnit = "";
            Unit.SelectedToUnit = "";
            IsResultLabelVisible = false;
        }

        // Currency: Get Rates
        [RelayCommand]
        public async Task GetRatesAsync()
        {
            var newBaseFrom = SplitBaseString(Unit?.SelectedFromUnit ?? "");

            Unit.CurrencyRate = await rateService.GetRates(newBaseFrom ?? "");

            if (Unit.CurrencyRate != null)
            {
                await ConvertRate();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Currency Rate cannot be null", "OK");
            }
        }

        // Currency: Convert Rate
        public async Task ConvertRate()
        {
            var newBaseTo = SplitBaseString(Unit?.SelectedToUnit ?? "");

            decimal convertRate = 0;

            switch (newBaseTo)
            {
                case "MXN":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.MXN != null ? Unit?.CurrencyRate?.Rate?.MXN : "");
                    break; // Curse you null safety ;)
                case "GBP":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.GBP != null ? Unit?.CurrencyRate.Rate.GBP : "");
                    break;
                case "EUR":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.EUR != null ? Unit?.CurrencyRate.Rate.EUR : "");
                    break;
                case "BTC":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.BTC != null ? Unit?.CurrencyRate.Rate.BTC : "");
                    break;
                case "CAD":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.CAD != null ? Unit?.CurrencyRate.Rate.CAD : "");
                    break;
                case "JPY":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.JPY != null ? Unit?.CurrencyRate.Rate.JPY : "");
                    break;
                case "RUB":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.RUB != null ? Unit?.CurrencyRate.Rate.RUB : "");
                    break;
                case "KRW":
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.KRW != null ? Unit?.CurrencyRate.Rate.KRW : "");
                    break;
                case "USD": // US Dollar
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.USD != null ? Unit?.CurrencyRate.Rate.USD : "");
                    break;
                case "HKD": // Hong Kong Dollar
                    convertRate = decimal.Parse(Unit?.CurrencyRate?.Rate?.HKD != null ? Unit?.CurrencyRate.Rate.HKD : "");
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Error", "No matching currency found", "OK");
                    break;
            }

            ConversionResult = (decimal.Parse(Unit?.UnitValue) * convertRate).ToString("F2");
            IsResultLabelVisible = true;

            CurrencyConversionSummary = rateService.GetConversionSummary(
                Unit?.CurrencyRate,
                Unit?.SelectedFromUnit,
                Unit?.SelectedToUnit
            );

            IsCurrencySummaryVisible = !string.IsNullOrWhiteSpace(CurrencyConversionSummary);
        }

        // Length: Convert Length
        public async Task ConvertLength()
        {
            var result = await lengthService.Convert(
                Unit?.LengthSelectedFromUnit,
                Unit?.LengthSelectedToUnit,
                Unit?.LengthUnitValue
            );

            if (result != null)
            {
                Unit.LengthConversionResult = result;
                IsLengthResultLabelVisible = true;
                LengthConversionSummary = lengthService.GetConversionSummary(
                    Unit?.LengthSelectedFromUnit,
                    Unit?.LengthSelectedToUnit
                );
            }
            else
            {
                Unit.LengthConversionResult = "Conversion not supported.";
                LengthConversionSummary = null;
                IsLengthResultLabelVisible = false;
            }
        }
    }
    public record TimeZoneItem(string Id, string DisplayName);
}

