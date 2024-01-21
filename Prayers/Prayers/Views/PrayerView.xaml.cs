using Pj.Library;
using Prayers.Services;
using Prayers.ViewModels;
using System;
using System.Collections.Generic;
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
            base.OnAppearing();

            if (PageId != null )
            {
                viewModel.SinglePageDataModel = SharedServices.PrayerViewModelData.SinglePageDataModels
                    .FirstOrDefault(f => f.PageId == PageId.ToInteger());
            }
            else
            {
                viewModel.SinglePageDataModel = SharedServices.PrayerViewModelData.SinglePageDataModels.First();
            }

            foreach (var item in viewModel.SinglePageDataModel.Content)
            {
                if (item.ContentType == "H")
                {
                    var para = new ParaHeaderView { ParaHeaderContent = item.Content, FontSize = item.FontSize };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P")
                {
                    //var para = new ParaJustifiedContentView { ParaContent = item.Content, FontSize = item.FontSize };
                    var para = new ParaContentView { ParaContent = item.Content, 
                        FontSize = item.FontSize, TextWrap = item.TextWrap };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Cont")
                {
                    var para = new ParaContinuationView { ParaContent = item.Content, FontSize = item.FontSize };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Bullet")
                {
                    var para = new ParaBulletItemView { ParaContent = item.Content, FontSize = item.FontSize };
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Bullet_No")
                {
                    var para = new ParaBulletNumberItemView { ParaContent = item.Content, ParaNumber = item.AdditionalData, FontSize = item.FontSize };
                    contentStack.Children.Add(para);
                }

            }
        }
    }
}