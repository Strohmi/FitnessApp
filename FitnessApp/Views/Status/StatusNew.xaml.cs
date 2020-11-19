using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitnessApp.Models.General;
using FitnessApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;

namespace FitnessApp
{
    public partial class StatusNew : ContentPage
    {
        public StatusVM StatusVM { get; set; }
        private bool isFoto;
        private bool isRichtig;

        public StatusNew()
        {
            InitializeComponent();
            StatusVM = new StatusVM();
            Start();
            BindingContext = StatusVM;
        }

        private void Start()
        {
            Title = "Status hinzufügen";
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Zurück");

            ToolbarItem item = new ToolbarItem()
            {
                Text = "Speichern",
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };
            item.Clicked += Save;
            ToolbarItems.Add(item);
        }

        private void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(beschreibung.Text))
            {
                StatusVM.Status.ErstelltVon = AllVM.ConvertToUser();
                StatusVM.Status.ErstelltAm = DateTime.Now;

                if (AllVM.Datenbank.Status.Insert(StatusVM.Status))
                {
                    OnBackButtonPressed();
                    DependencyService.Get<IMessage>().ShortAlert("Erfolgreich gespeichert");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Fehler beim Speichern");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Beschreibung ausfüllen");
            }
        }

        private async void UploadPhoto(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 50,
                    SaveMetaData = false,
                });

                if (photo != null)
                {
                    using (FileStream fs = File.OpenRead(photo.Path))
                    {
                        fs.CopyTo(ms);
                        StatusVM.Status.Foto = ms.ToArray();
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
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    DefaultCamera = CameraDevice.Front,
                    AllowCropping = true,
                    SaveMetaData = false,
                    CompressionQuality = 50
                });

                if (photo != null)
                {
                    using (FileStream fs = File.OpenRead(photo.Path))
                    {
                        fs.CopyTo(ms);
                        StatusVM.Status.Foto = ms.ToArray();
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

        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }
    }
}