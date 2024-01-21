using Newtonsoft.Json;
using Prayers.Models;
using Prayers.ViewModels.Extras;
using Prayers.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prayers.ViewModels
{
    public class PrayerViewModel : BaseViewModel
    {
        public ICommand btnGoPreviousCommand { get; set; }
        public ICommand btnGoNextCommand { get; set; }

        string _pageId;
        public string PageId
        {
            get { return _pageId; }
            set
            {
                _pageId = value;
                OnPropertyChanged(nameof(PageId));
            }
        }

        private SinglePageDataModel _singlePageDataModel;
        public SinglePageDataModel SinglePageDataModel
        {
            get => _singlePageDataModel;
            set
            {
                _singlePageDataModel = value;
                if (value != null)
                {
                    PageId = value.PageId.ToString();
                }
                OnPropertyChanged(nameof(SinglePageDataModel));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

        public PrayerViewModel()
        {
            btnGoPreviousCommand = new Command(async () => await GoPrevious());
            btnGoNextCommand = new Command(async () => await GoNext());
        }

        public async Task GoPrevious()
        {
            if (CanGoPrevious)
            {
                //var route = $"{nameof(PrayerView)}?PageId=1";
                //var route = $"Page2?PageId=1";
                var route = $"Page{SinglePageDataModel.PreviousPageId}?PageId={SinglePageDataModel.PreviousPageId}";
                await Shell.Current.GoToAsync(route);
            }
        }

        public async Task GoNext()
        {
            if (CanGoNext)
            {
                //var route = $"//{nameof(PrayerView)}?PageId=1";
                var route = $"Page{SinglePageDataModel.NextPageId}?PageId={SinglePageDataModel.NextPageId}";

                await Shell.Current.GoToAsync(route);
            }
        }

        public bool CanGoPrevious
        {
            //get => true;
            get
            {
                return SinglePageDataModel?.PreviousPageId != null && SinglePageDataModel?.PreviousPageId.HasValue == true;
            }
            //get { return currentPageIndex > 0; }
        }

        public bool CanGoNext
        {
            get => SinglePageDataModel?.NextPageId != null && SinglePageDataModel?.NextPageId.HasValue == true;
            //get => true;
            //get { return currentPageIndex < pages.Count - 1; }
        }

        [JsonIgnore]
        private StyleModelDefault styleModelDefault;
        [JsonIgnore]
        public StyleModelDefault DefaultStyle
        {
            get => styleModelDefault;
            set
            {
                styleModelDefault = value;
                OnPropertyChanged("DefaultStyle");
            }
        }
    }
}
