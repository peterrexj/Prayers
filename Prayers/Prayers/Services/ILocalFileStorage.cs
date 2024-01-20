using Prayers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prayers.Services
{
    public interface ILocalFileStorage
    {
        string PrayerEmbeddedDataFilePath { get; }
        IEnumerable<PrayerRawDataModel> PrayerEmbeddedData { get; }


        Task WriteTextAsync(string fileName, string text);
        Task<string> ReadTextAsync(string fileName);

        string SettingsDataFilePath { get; }
        string DefaultDataFilePath { get; }

        string FilePathBasedOnType<T>();
        Task<T> GetData<T>();
        Task SaveData<T>(T data);
        Task ClearData<T>();
    }
}
