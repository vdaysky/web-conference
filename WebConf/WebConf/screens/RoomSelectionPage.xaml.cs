using common;
using NetworkCommon.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebConf.network.packet;

namespace WebConf.screens
{
    /// <summary>
    /// Логика взаимодействия для RoomSelectionPage.xaml
    /// </summary>
    public partial class RoomSelectionPage : Page
    {
        public RoomSelectionPage()
        {
            InitializeComponent();
            initAsync();
            App.Model.OnRoomCreated += addRoom;
        }

        public async void initAsync()
        {
            List<Room> rooms = await App.Model.GetRooms();
            foreach (var room in rooms)
            {
                addRoom(room);
            }
        }

        private void addRoom(Room room) {
            ListViewItem list_item = new ListViewItem();
            list_item.Tag = room.id;
            list_item.Content = room.name;
            RoomsList.Items.Add(list_item);
        }

        private void RoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var window = App.getWindow();

            long room_id = (long) ((ListViewItem)RoomsList.SelectedItem).Tag;
            App.Navigate(new RoomPage(room_id));
        }

        private async void CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            string roomName = RoomNameInput.Text;
            Room room = await App.Model.CreateRoom(roomName);
        }

        private void onKeyDown(object sender, RoutedEventArgs e) {

        }
    }
}
