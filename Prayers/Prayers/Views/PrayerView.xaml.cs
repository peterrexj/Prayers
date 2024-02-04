using Pj.Library;
using Prayers.Extensions;
using Prayers.Services;
using Prayers.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prayers.Views
{
    [QueryProperty(nameof(PageId), nameof(PageId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayerView : ContentPage
    {
        public string PageId { get; set; }

        PrayerViewModel viewModel;
        public PrayerView()
        {
            InitializeComponent();

            viewModel = new PrayerViewModel();

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            viewModel.DefaultStyle = ThemeHelper.GetDefaultStyleTheme();

            base.OnAppearing();

            int currentPageId = 0;
            if (PageId != null)
            {
                currentPageId = PageId.ToInteger();
            }
            else
            {
                currentPageId = SharedServices.PrayerViewModelData.SinglePageDataModels.First().PageId;
            }

            viewModel.SinglePageDataModel = SharedServices.PrayerViewModelData.SinglePageDataModels
                    .FirstOrDefault(f => f.PageId == currentPageId);

            viewModel.ProgressInformation = new ObservableCollection<PrayerProgressModel>(
                SharedServices.PrayerViewModelData.SinglePageDataModels
                    .Where(f => f.PageId < currentPageId)
                    .Select(f =>
                        new PrayerProgressModel
                        {
                            Title = "",
                            ProgressValue = 100,
                            Status = Syncfusion.XForms.ProgressBar.StepStatus.Completed
                        })
                .Union(SharedServices.PrayerViewModelData.SinglePageDataModels
                    .Where(f => f.PageId == currentPageId)
                    .Select(f =>
                        new PrayerProgressModel
                        {
                            Title = "",
                            ProgressValue = 0,
                            Status = Syncfusion.XForms.ProgressBar.StepStatus.InProgress
                        }))
                .Union(SharedServices.PrayerViewModelData.SinglePageDataModels
                    .Where(f => f.PageId > currentPageId)
                    .Select(f =>
                        new PrayerProgressModel
                        {
                            Title = "",
                            ProgressValue = 0,
                            Status = Syncfusion.XForms.ProgressBar.StepStatus.NotStarted
                        }))
                );



            //viewModel.ProgressInformation = new ObservableCollection<PrayerProgressModel>(SharedServices.PrayerViewModelData.SinglePageDataModels.Select(f =>
            //    new PrayerProgressModel
            //    {
            //        Title = "",
            //        ProgressValue = 0,
            //        Status = Syncfusion.XForms.ProgressBar.StepStatus.Completed
            //    }));

            foreach (var item in viewModel.SinglePageDataModel.Content)
            {
                if (item.ContentType == "H")
                {
                    var para = new ParaHeaderView { ParaHeaderContent = item.Content, 
                        FontSize = item.FontSize,
                    };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P")
                {
                    var para = new ParaContentView
                    {
                        ParaContent = item.Content,
                        FontSize = item.FontSize,
                        TextWrap = item.TextWrap,
                        FontAlign = item.FontAlign,
                        FontCustomAttributes = item.FontAttribute,
                    };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Bullet")
                {
                    var para = new ParaBulletView
                    {
                        ParaContent = item.Content,
                        ParaNumber = item.AdditionalData,
                        FontSize = item.FontSize,
                        TextWrap = item.TextWrap,
                        FontAlign = item.FontAlign,
                        FontCustomAttributes = item.FontAttribute
                    };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Img")
                {
                    var para = new ParaImageView
                    {

                    };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "Sound")
                {
                    viewModel.AudioFileName = item.Content.Trim();
                }

            }
        }
    }
}