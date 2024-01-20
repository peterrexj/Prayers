using Pj.Library;
using Prayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Prayers.Services
{
    public static class SharedServices
    {

        public static void LoadPrayerViewModelData()
        {
            if (PrayerViewModelData?.SinglePageDataModels.Count == null)
            {
                //take an action
            }
        }

        static PrayerAppModel _prayerViewModelData;
        public static PrayerAppModel PrayerViewModelData
        {
            get
            {
                if (_prayerViewModelData == null)
                {
                    var prayerRawData = DependencyService.Get<ILocalFileStorage>().PrayerEmbeddedData;
                    if (prayerRawData != null)
                    {
                        //throw some message to user about data load failure
                    }

                    _prayerViewModelData = new PrayerAppModel
                    {
                        MainHeaders = new List<string>(),
                        SinglePageDataModels = new List<SinglePageDataModel>()
                    };

                    var grpData = prayerRawData.GroupBy(f => f.HeaderSequenceId)
                        .Select(f => new { f.Key, PageData = f.OrderBy(g => g.SequenceId).ToList() })
                        .ToList();

                    foreach (var data in grpData.Where(f => f.Key == -1)
                        .SelectMany(f => f.PageData)
                        .OrderBy(f => f.SequenceId))
                    {
                        if (data.ContentType == "MH")
                        {
                            _prayerViewModelData.MainHeaders.Add(data.Content.Trim());
                        }
                    }


                    int currentPageId = 0;
                    foreach (var data in grpData.Where(f => f.Key != -1))
                    {
                        currentPageId++;
                        var pageData = new SinglePageDataModel
                        {
                            PageId = currentPageId,
                            Content = new List<PageContentModel>()
                        };
                        if (currentPageId == 1)
                        {
                            pageData.PreviousPageId = null;
                        }
                        else
                        {
                            pageData.PreviousPageId = currentPageId - 1;
                            _prayerViewModelData.SinglePageDataModels.Last().NextPageId = currentPageId;
                        }

                        foreach (var item in data.PageData)
                        {

                            pageData.Content.Add(new PageContentModel
                            {
                                ContentType = item.ContentType,
                                Content = item.Content.Trim(),
                                FontSize = item.FontSize == null ? 12 : item.FontSize.Value,
                                FontAlign = item.FontAlign.HasValue() ? item.FontAlign.Trim() : "Right",
                                FontAttribute = item.FontAttribute,
                                TextWrap = item.TextWrap.HasValue() ? item.TextWrap : "Normal",
                                AdditionalData = item.AdditionalData,
                                
                                IsHeader = item.ContentType.HasValue() && item.ContentType == "H"
                            });
                        }

                        _prayerViewModelData.SinglePageDataModels.Add(pageData);
                    }
                }
                return _prayerViewModelData;
            }
        }


        static ILocalFileStorage _localStorage;

        public static ILocalFileStorage LocalStorage => _localStorage ?? DependencyService.Get<ILocalFileStorage>();
    }
}
