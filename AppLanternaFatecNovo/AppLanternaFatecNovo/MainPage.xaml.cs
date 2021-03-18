using System;
using Xamarin.Essentials;
using Xamarin.Forms;

using Plugin.Battery;
using Plugin.Battery.Abstractions;

namespace AppLanternaFatecNovo
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;


        public MainPage()
        {
            InitializeComponent();

            btnOnOff.Source = ImageSource.FromResource("AppLanternaFatecNovo.Image.botao-desligado.jpg");

            Carrega_Info_Bateria();
        }


        private void Carrega_Info_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    // Disp deu a permissão...
                    CrossBattery.Current.BatteryChanged -= Mudanca_status_bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_status_bateria;
                }
                else
                    throw new Exception("Sem permissão para dados da bateria.");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private void Mudanca_status_bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_carga.Text = e.RemainingChargePercent.ToString() + "%";

                if(e.IsLow)
                {
                    lbl_bateria_fraca.Text = "Bateria Fraca!";
                    lbl_bateria_fraca.TextColor = Color.Red;
                    lbl_bateria_fraca.HorizontalTextAlignment = TextAlignment.Center;
                    lbl_bateria_fraca.FontAttributes = FontAttributes.Italic;
                }

                switch(e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido.";
                        break;
                }


                switch(e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_source.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_source.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_source.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_source.Text = "Sem Fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_source.Text = "Desconhecido";
                        break;
                }

            } catch(Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "Ok");
            }
        }


        private void btnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(lanterna_ligada)
                {
                    // situação para desligar.
                    Flashlight.TurnOffAsync();
                    lanterna_ligada = false;
                    btnOnOff.Source = ImageSource.FromResource("AppLanternaFatecNovo.Image.botao-desligado.jpg");

                } else
                {
                    // situação para ligar.
                    Flashlight.TurnOnAsync();
                    lanterna_ligada = true;
                    btnOnOff.Source = ImageSource.FromResource("AppLanternaFatecNovo.Image.botao-ligado.jpg");
                }                

                Vibration.Vibrate(TimeSpan.FromMilliseconds(300));

            } catch(Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
