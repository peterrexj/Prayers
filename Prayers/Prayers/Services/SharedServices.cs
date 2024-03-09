using Pj.Library;
using Prayers.Converters;
using Prayers.Extensions;
using Prayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

                    try
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
                            else if (data.ContentType == "MHT")
                            {
                                _prayerViewModelData.MainTitle = data.Content.Trim() ?? "";
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
                                var tempData = new PageContentModel
                                {
                                    ContentType = item.ContentType,
                                    FontSize = item.FontSize == null ? 12 : item.FontSize.Value,
                                    FontAlign = item.FontAlign.HasValue() ? item.FontAlign.Trim() : "Right",
                                    FontAttribute = item.FontAttribute?.SplitAndTrim(",").ToArray() ?? new string[] { },
                                    TextWrap = item.TextWrap.HasValue() ? item.TextWrap : "Normal",
                                    AdditionalData = item.AdditionalData,
                                    IsHeader = item.ContentType.HasValue() && item.ContentType == "H"
                                };

                                if (item.SpaceBefore.HasValue && item.SpaceBefore.Value > 0)
                                {
                                    if (item.FontAlign.HasValue() && item.FontAlign.Trim().EqualsIgnoreCase("Right"))
                                    {
                                        tempData.Content = item.Content.Trim().PadLeft(item.Content.Trim().Length + item.SpaceBefore.Value);
                                    }
                                    else
                                    {
                                        tempData.Content = item.Content.Trim().PadRight(item.Content.Trim().Length + item.SpaceBefore.Value);
                                    }
                                }
                                else
                                {
                                    tempData.Content = item.Content.Trim();

                                }
                                pageData.Content.Add(tempData);
                            }

                            _prayerViewModelData.SinglePageDataModels.Add(pageData);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.CaptureException(ex, "SharedServices.PrayerViewModelData");
                    }
                }
                return _prayerViewModelData;
            }
        }

        static ILocalFileStorage _localStorage;
        public static ILocalFileStorage LocalStorage => _localStorage ?? DependencyService.Get<ILocalFileStorage>();

        static AudioController audioController;
        public static AudioController AudioController => audioController ?? new AudioController();

        static StringToColorConverter _toColorConverter;
        public static StringToColorConverter ToColorConverter => _toColorConverter ??= new StringToColorConverter();
        public static IValueConverter ToColorConverterAsValueConverter => ToColorConverter;

        private static string _pathToMainImage;
        public static string PathToMainImage
        {
            get
            {
                if (_pathToMainImage.IsEmpty())
                {
                    if (Device.RuntimePlatform ==  Device.Android)
                    {
                        _pathToMainImage = "drawable/mainpic.jpg";
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        _pathToMainImage = "mainpic.jpg";
                    }
                    else if (Device.RuntimePlatform == Device.UWP)
                    {
                        _pathToMainImage = "Assets/mainpic.jpg";
                    }
                }
                return _pathToMainImage;
            }
        }
    }
}
