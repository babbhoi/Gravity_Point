using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace dot2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class gameOver : Page
    {

        StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        StorageFile sampleFile1;
        StorageFile sampleFile;
        string text;
        public gameOver()
        {
            this.InitializeComponent();
            
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             text = e.Parameter as string;
            if (text != null)
            {
                score.Text = text;
            }
            SaveFile_Initialize();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Canvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
        private async void SaveFile_Initialize()
        {
            sampleFile1 = await storageFolder.CreateFileAsync("score.txt", CreationCollisionOption.OpenIfExists);

            sampleFile = await storageFolder.GetFileAsync("score.txt");
            string read_ = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            if (read_ == "")
            {
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile1, text);
                hs.Text = text;
            }
            else
            {
                int num = Int32.Parse(read_);
                int new_high_score = Int32.Parse(text);
                if (new_high_score > num)
                {
                    await Windows.Storage.FileIO.WriteTextAsync(sampleFile1, text);
                    hs.Text = text;
                }
                else
                {
                    hs.Text = num.ToString() ;
                }
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
       
    }
}
