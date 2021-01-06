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
    //Autor: Niklas Erichsen

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

        /// <summary>
        /// Methode, die erst nach dem Laden ausgeführt werden soll
        /// </summary>
        private void Loaded(object sender, EventArgs e)
        {
            pickPhoto = false;
            BindingContext = ProfilVM;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
        private void Start()
        {
            Title = "Profil bearbeiten";
            SetNavBar();
        }

        /// <summary>
        /// Einstellungen der Navigationsbar
        /// </summary>
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

        /// <summary>
        /// Änderungen des Profils speichern und an die Datenbank senden
        /// </summary>
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

        /// <summary>
        /// Auswahl, welches Foto hochgeladen werden soll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Auswahl eines Fotos aus der Mediathek des Gerätes 
        /// </summary>
        private async void UploadPhoto()
        {
            try
            {
                pickPhoto = true;
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Large,
                    CompressionQuality = 100,
                    SaveMetaData = false,
                });

                if (photo != null)
                {
                    photo.GetStreamWithImageRotatedForExternalStorage().CopyTo(ms);
                    ProfilVM.User.ProfilBild = ms.ToArray();
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

        /// <summary>
        /// Aufnehmen eines Fotos mit der Kamera des Gerätes
        /// </summary>
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
                    CompressionQuality = 100,
                    PhotoSize = PhotoSize.Large
                });

                if (photo != null)
                {
                    photo.GetStreamWithImageRotatedForExternalStorage().CopyTo(ms);
                    ProfilVM.User.ProfilBild = ms.ToArray();
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

        /// <summary>
        /// Profilbild zum Standard-Bild setzen
        /// </summary>
        void DeletePhoto(System.Object sender, System.EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                ProfilVM.User.ProfilBild = webClient.DownloadData("https://cdn.pixabay.com/photo/2016/11/11/09/59/white-male-1816195_1280.jpg");
            }
        }

        /// <summary>
        /// Nutzer komplett löschen
        /// </summary>
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

        /// <summary>
        /// Springen zum Passwort ändern
        /// </summary>
        void ChangePWD(System.Object sender, System.EventArgs e)
        {
            this.Navigation.PushAsync(new ChangePassword(ProfilVM.User));
        }

        /// <summary>
        /// Seite aus dem Stack löschen
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }

        /// <summary>
        /// Aktuellen Benutzer abmelden und zum Login springen
        /// </summary>
        void Logoff(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Login());
        }
    }
}