using System;
using Xamarin.Forms;
using FitnessApp.Models.General;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;

namespace FitnessApp
{
    //Autor: NiE

    public partial class StatusNew : ContentPage
    {
        public StatusVM StatusVM { get; set; }

        public StatusNew()
        {
            InitializeComponent();
            StatusVM = new StatusVM();
            Start();
            BindingContext = StatusVM;
        }

        /// <summary>
        /// Startmethode für bessere Übersicht, die am Anfang ausgeführt werden müssen
        /// </summary>
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

        /// <summary>
        /// Status speichern und zur Datenbank übertragen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e)
        {
            //Prüfen, damit keine NULL-Werte in die Datenbank geschrieben werden
            if (!string.IsNullOrWhiteSpace(beschreibung.Text))
            {
                StatusVM.Status.ErstelltVon = AllVM.ConvertToUser();
                StatusVM.Status.ErstelltAm = DateTime.Now;

                //Prüfen, ob das Speichern in die Datenbank erfolgreich ist
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

        /// <summary>
        /// Auswahl wie das Foto ausgewählt werden soll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void UploadPhoto(System.Object sender, System.EventArgs e)
        {
            var result = await DisplayActionSheet("Auswahl", "Abbrechen", null, new string[] { "Aufnehmen", "Auswählen" });

            switch (result)
            {
                case "Aufnehmen":
                    TakePhoto();
                    break;
                case "Auswählen":
                    PickPhoto();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Auswahl eines Fotos aus der Mediathek des Gerätes
        /// </summary>
        private async void PickPhoto()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Large,
                    CompressionQuality = 100,
                    SaveMetaData = false,
                });

                if (photo != null)
                {
                    //Speichern als ByteArray
                    photo.GetStreamWithImageRotatedForExternalStorage().CopyTo(ms);
                    StatusVM.Status.Foto = ms.ToArray();
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
                    //Speichern als ByteArray
                    photo.GetStreamWithImageRotatedForExternalStorage().CopyTo(ms);
                    StatusVM.Status.Foto = ms.ToArray();
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
        /// Überschreiben der Standardmethode, damit die Seite aus dem Stack gelöscht wird
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            this.Navigation.PopAsync();
            return true;
        }
    }
}