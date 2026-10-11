using NUnit.Framework;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Tests;

[TestFixture]
public class ViewModelTests
{
    [Test]
    public void SettingsViewModel_DefaultTheme_IsDark()
    {
        var vm = new SettingsViewModel { SelectedTheme = "Dark" };
        Assert.That(vm.SelectedAppTheme, Is.EqualTo(AppThemes.Dark));
    }

    [Test]
    public void SettingsViewModel_LightTheme_ParsesCorrectly()
    {
        var vm = new SettingsViewModel { SelectedTheme = "Light" };
        Assert.That(vm.SelectedAppTheme, Is.EqualTo(AppThemes.Light));
    }

    [Test]
    public void BaseViewModel_IsFree_IsInverseOfIsBusy()
    {
        var vm = new BaseViewModel();
        Assert.That(vm.IsFree, Is.True);
        vm.IsBusy = true;
        Assert.That(vm.IsFree, Is.False);
        vm.IsBusy = false;
        Assert.That(vm.IsFree, Is.True);
    }

    [Test]
    public void StepProgressBarItem_DefaultStatus_IsNotStarted()
    {
        var m = new Syncfusion.Maui.ProgressBar.StepProgressBarItem();
        Assert.That(m.Status, Is.EqualTo(Syncfusion.Maui.ProgressBar.StepStatus.NotStarted));
    }

    [Test]
    public void MainViewModel_PathToMainImage_IsNotEmpty()
    {
        var vm = new MainViewModel();
        Assert.That(vm.PathToMainImage, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void PrayerViewModel_CanGoPrevious_WhenNoPreviousPage_IsFalse()
    {
        var vm = new PrayerViewModel
        {
            SinglePageDataModel = new SinglePageDataModel { PageId = 1, PreviousPageId = null }
        };
        Assert.That(vm.CanGoPrevious, Is.False);
    }

    [Test]
    public void PrayerViewModel_CanGoNext_WhenNextPageExists_IsTrue()
    {
        var vm = new PrayerViewModel
        {
            SinglePageDataModel = new SinglePageDataModel { PageId = 1, NextPageId = 2 }
        };
        Assert.That(vm.CanGoNext, Is.True);
    }
}
