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

            //Entfernen nach Auto-Login !
            if (ProfilVM.User.ProfilBild == null)
                using (var webClient = new WebClient())
                {
                    ProfilVM.User.ProfilBild = webClient.DownloadData("https://cdn.pixabay.com/photo/2016/11/11/09/59/white-male-1816195_1280.jpg");
                }

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
            //ProfilVM.User.Nutzername = nutzername.Text;
            ProfilVM.User.InfoText = infoText.Text;

            AllVM.User = AllVM.ConvertFromUser(ProfilVM.User);

            //if (AllVM.Datenbank.User.UploadProfilBild(ProfilVM.User, ProfilVM.User.ProfilBild))
            //{
            //    saved = true;
            //    OnBackButtonPressed();
            //    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gespeichert");
            //}
            //else
            //{
            //    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
            //}

            if (AllVM.Datenbank.User.Update(ProfilVM.User))
            {
                saved = true;
                OnBackButtonPressed();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gespeichert");
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
            }
        }

        private async void UploadPhoto(object sender, EventArgs e)
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

        private async void TakePhoto(object sender, EventArgs e)
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

        void DeleteUser(System.Object sender, System.EventArgs e)
        {
            if (AllVM.Datenbank.User.Delete(AllVM.ConvertToUser()))
            {
                Application.Current.MainPage = new Login();
                DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gelöscht");
            }
            else
                DependencyService.Get<IMessage>().ShortAlert("Fehler beim Löschen");
        }

        void ChangeStatus(System.Object sender, System.EventArgs e)
        {

        }

        async void ChangePWD(System.Object sender, System.EventArgs e)
        {
            string pwAlt = await DisplayPromptAsync("Aktuelles Passwort", null);

            if (!string.IsNullOrWhiteSpace(pwAlt))
            {
                //Passwort hashen MD5 und dann Vergleich mit den aus der Datenbank
                string hashpw = null;

                if (hashpw == pwAlt)
                {
                    string pw1 = await DisplayPromptAsync("Neues Passwort", null);

                    if (!string.IsNullOrWhiteSpace(pw1))
                    {
                        string pw2 = await DisplayPromptAsync("Passwort wiederholen", null);

                        if (!string.IsNullOrWhiteSpace(pw2))
                        {
                            if (pw1 == pw2)
                            {
                                //Passwort hashen MD5
                                //Passwort in Datenbank UPDATE
                                //Passwort in AllVM.User
                            }
                            else
                                DependencyService.Get<IMessage>().ShortAlert("Passwörter stimmen nicht überein");
                        }
                    }
                }
                else
                    DependencyService.Get<IMessage>().ShortAlert("Aktuelles Passwort stimmt nicht");
            }
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
    }
}