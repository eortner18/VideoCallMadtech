﻿using MadTechLib;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twilio;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Video.V1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                TwilioClient.Init("ACb384c79bf65f369fde65c47460944e8d", "5e42bb265e61d3786dfcb5f28c974333");

                var room = RoomResource.Create(uniqueName: "MyRoom", emptyRoomTimeout: 60);
                Console.WriteLine(room);
                int count = RoomResource.Read().Count();
                Console.WriteLine(count);
                Title = RoomResource.Read().Count().ToString();

            }catch(Exception ex)
            {
                Title = ex.Message;
            }
        }
    }
}