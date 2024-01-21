using Pj.Library;
using Prayers.Extensions;
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
            viewModel.DefaultStyle = ThemeHelper.GetDefaultStyleTheme();

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
                    var para = new ParaContentView
                    {
                        ParaContent = item.Content,
                        FontSize = item.FontSize,
                        TextWrap = item.TextWrap,
                        FontAlign = item.FontAlign,
                        FontCustomAttributes = item.FontAttribute
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

            }
        }
    }
}