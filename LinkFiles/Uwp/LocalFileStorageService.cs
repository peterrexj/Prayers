using CsvHelper;
using Prayers.Models;
using Prayers.Services;
using Prayers.UWP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

[assembly: Dependency(typeof(LocalFileStorageService))]
namespace Prayers.UWP.Services
{
    public class LocalFileStorageService : ILocalFileStorage
    {

        public IEnumerable<PrayerRawDataModel> PrayerEmbeddedData => ReadCsvFile<PrayerRawDataModel>(DependencyService.Get<IAppInformation>().PrayerEmbeddedDataFilePath).AsEnumerable();

        public static List<T> ReadCsvFile<T>(string path)
        {
            try
            {
                using (var reader = new StreamReader(
                    File.OpenRead(
                        Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets", path))))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SettingsDataFilePath => Path.Combine(ApplicationData.Current.LocalFolder.Path, "settingsdata.json");

        public string DefaultDataFilePath => Path.Combine(ApplicationData.Current.LocalFolder.Path, "defaultdata.json");

        public async Task ClearData<T>()
        {
            await Task.Run(() => {
                File.Delete(FilePathBasedOnType<T>());
                return Task.CompletedTask;
            });
        }

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

        public async Task<string> ReadTextAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(fileName);
            using (var stream = await file.OpenStreamForReadAsync())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        public async Task SaveData<T>(T data)
        {
            await Task.Run(() => {
                Pj.Library.SerializationHelper.SerializeToJson<T>(data, FilePathBasedOnType<T>());
                return Task.CompletedTask;
            });
        }

        public async Task WriteTextAsync(string fileName, string text)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    await streamWriter.WriteAsync(text);
                }
            }
        }
    }
}
