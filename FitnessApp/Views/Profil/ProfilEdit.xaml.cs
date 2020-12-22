using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using FitnessApp.Models;
using FitnessApp.Models.General;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp
{
    //Autor: NiE

    public partial class ProfilEdit : ContentPage
    {
        public ProfilEditVM ProfilVM { get; set; }
        private bool saved;
        private bool pickPhoto;

        public ProfilEdit()
        {
            InitializeComponent();
            ProfilVM = new ProfilEditVM(AllVM.ConvertToUser());
            Start();
        }

        private void Loaded(object sender, EventArgs e)
        {
            pickPhoto = false;
            BindingContext = ProfilVM;
        }

        private void Start()
        {
            Title = "Profil bearbeiten";
            SetNavBar();
        }

        private void SetNavBar()
        {
            ToolbarItem item = new ToolbarItem
            {
                Text = "Speichern",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            ProfilVM.User.OnlyCustomName = onlyCustomName.IsChecked;
            ProfilVM.User.CustomName = customName.Text;
            ProfilVM.User.InfoText = infoText.Text;

            if (AllVM.Datenbank.User.Update(ProfilVM.User))
            {
                AllVM.User = AllVM.ConvertFromUser(ProfilVM.User);
                saved = true;
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gespeichert");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
        }

        private async void UploadPhoto()
        {
            try
            {
                pickPhoto = true;
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 30,
                    SaveMetaData = false,
                });

                if (photo != null)
                {
                    using (FileStream fs = File.OpenRead(photo.Path))
                    {
                        fs.CopyTo(ms);
                        ProfilVM.User.ProfilBild = ms.ToArray();
                    }
                }
            }
            catch (NotSupportedException ex1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Foto auswählen wird nicht unterstützt");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert("Es ist ein Fehler aufgetreten");
            }
        }

        private async void TakePhoto()
        {
            try
            {
                pickPhoto = true;
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    DefaultCamera = CameraDevice.Front,
                    AllowCropping = true,
                    SaveMetaData = false,
                    CompressionQuality = 30
                });

                if (photo != null)
                {
                    using (FileStream fs = File.OpenRead(photo.Path))
                    {
                        fs.CopyTo(ms);
                        ProfilVM.User.ProfilBild = ms.ToArray();
                    }
                }
            }
            catch (NotSupportedException ex1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Foto aufnehmen wird nicht unterstützt");
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                DependencyService.Get<IMessage>().ShortAlert("Es ist ein Fehler aufgetreten");
            }
        }

        void DeletePhoto(System.Object sender, System.EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                ProfilVM.User.ProfilBild = webClient.DownloadData("https://cdn.pixabay.com/photo/2016/11/11/09/59/white-male-1816195_1280.jpg");
            }
        }

        async void DeleteUser(System.Object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Löschen?", "Möchtest du deinen Account wirklich löschen?\nAlle deine Daten werden nicht mehr zugänglich sein!", "Ja", "Nein"))
            {
                if (AllVM.Datenbank.User.Delete(AllVM.ConvertToUser()))
                {
                    Application.Current.MainPage = new Login();
                    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
            }
        }

        void ChangePWD(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new ChangePassword(ProfilVM.User));
        }

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }

        void Logoff(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Login());
        }

        async void ChangePhoto(System.Object sender, System.EventArgs e)
        {
            var result = await DisplayActionSheet("Profilbild ändern", "Abbrechen", null, new string[] { "Aufnehmen", "Hochladen" });
            switch (result)
            {
                case "Aufnehmen":
                    TakePhoto();
                    break;
                case "Hochladen":
                    UploadPhoto();
                    break;
                default:
                    break;
            }
        }
    }
}