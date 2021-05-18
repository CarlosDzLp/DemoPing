using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoPing
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var resp = await DependencyService.Get<IPing>().Ping("https://github.com/xamarin/XamarinComponents/tree/main/iOS/SimplePing");
                lblping.Text = resp;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
