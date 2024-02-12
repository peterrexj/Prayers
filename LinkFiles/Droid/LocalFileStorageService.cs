using CsvHelper;
using Prayers.Droid.Services;
using Prayers.Models;
using Prayers.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileStorageService))]
namespace Prayers.Droid.Services
{
    public class LocalFileStorageService : ILocalFileStorage
    {
        public IEnumerable<PrayerRawDataModel> PrayerEmbeddedData => ReadCsvFile<PrayerRawDataModel>(DependencyService.Get<IDataSource>().PrayerEmbeddedDataFilePath).AsEnumerable();

        public static List<T> ReadCsvFile<T>(string path)
        {
            try
            {
                using var reader = new StreamReader(Android.App.Application.Context.Assets.Open(path));
                using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
                return csv.GetRecords<T>().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task WriteTextAsync(string fileName, string text)
        {
            var file = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), fileName);
            using (var streamWriter = new StreamWriter(file, false))
            {
                await streamWriter.WriteAsync(text);
            }
        }
        public async Task<string> ReadTextAsync(string fileName)
        {
            var file = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), fileName);
            using (var streamReader = new StreamReader(file))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public string DefaultDataFilePath =>
          Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "defaultdata.json");
        public string SettingsDataFilePath =>
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "settingsdata.json");

        public string FilePathBasedOnType<T>()
        {
            switch (typeof(T).Name)
            {
                case "SettingsViewModel":
                    return SettingsDataFilePath;
                default:
                    return DefaultDataFilePath;
            }
        }
        public async Task<T> GetData<T>()
        {
            return await Task.Run(() =>
            {
                T viewModel;
                if (File.Exists(FilePathBasedOnType<T>()) == false)
                {
                    return Task.FromResult<T>(default(T));
                }
                else
                {
                    viewModel = Pj.Library.SerializationHelper.DeSerializeFromJsonFile<T>(FilePathBasedOnType<T>());
                }

                return Task.FromResult(viewModel);
            });
        }
        public async Task SaveData<T>(T data)
        {
            await Task.Run(() =>
            {
                Pj.Library.SerializationHelper.SerializeToJson<T>(data, FilePathBasedOnType<T>());
                return Task.CompletedTask;
            });
        }
        public async Task ClearData<T>()
        {
            await Task.Run(() =>
            {
                File.Delete(FilePathBasedOnType<T>());
                return Task.CompletedTask;
            });
        }
    }
}