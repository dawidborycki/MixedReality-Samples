#region Using

using MixedReality.Common.ViewModels;
using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;

#endregion

namespace UWP.Basics
{
    public class PersonViewModel : BaseViewModel
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public int Age { get; set; }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }

        private string firstName;
        private string lastName;
        private int age;
     
        private ApplicationDataContainer localSettings =
            ApplicationData.Current.LocalSettings;

        private string firstNameKey = nameof(FirstName);
        private string lastNameKey = nameof(LastName);
        private string ageKey = nameof(Age);

        public PersonViewModel()
        {
            if (IsPersonDataStoredInSettings())
            {
                FirstName = localSettings.Values[firstNameKey].ToString();
                LastName = localSettings.Values[lastNameKey].ToString();
                Age = (int)localSettings.Values[ageKey];
            }
        }

        public void StoreInSettings()
        {
            localSettings.Values[firstNameKey] = FirstName;
            localSettings.Values[lastNameKey] = LastName;
            localSettings.Values[ageKey] = Age;
        }

        public async void DisplayPersonData(object sender, TappedRoutedEventArgs e)
        {
            var message = $"{FirstName} {LastName}";

            var messageDialog = new MessageDialog(message, "Person data:");

            await messageDialog.ShowAsync();
        }

        private bool IsPersonDataStoredInSettings()
        {
            string[] keys = { firstNameKey, lastNameKey, ageKey };

            var isPersonDataStoredInSettings = true;

            foreach (var key in keys)
            {
                if (!localSettings.Values.ContainsKey(key))
                {
                    isPersonDataStoredInSettings = false;
                    break;
                }
            }

            return isPersonDataStoredInSettings;
        }
    }
}
