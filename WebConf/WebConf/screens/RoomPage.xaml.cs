using common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

class MessageGUIState {
    public long message_id;
    public bool expanded = false;
    public MessageGUIState(long id) {
        message_id = id;
    }
}
namespace WebConf.screens
{
    /// <summary>
    /// Логика взаимодействия для RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        private long room_id;

        private TreeViewItem findMessageItem(TreeViewItem parent, long message_id) {
            foreach (TreeViewItem item in parent.Items) {
                MessageGUIState state = (MessageGUIState)item.Tag;
                if (state.message_id == message_id) {
                    return item;
                }
                TreeViewItem x = findMessageItem(item, message_id);
                if (x != null) {
                    return x;
                }
            }
            return null;
        }

        private void onMessageAdded(Message message) {
            
            if (message.parent_id != null) {
                TreeViewItem item = findMessageItem(RoomMsgContainer, (int) message.parent_id);

                if (item == null) {
                    return;
                }

                addMessage(item, message);
                return;
            }
            addMessage(null, message);
        }

        public RoomPage(long room_id)
        {
            InitializeComponent();
            this.room_id = room_id;
            initAsync();
            App.Model.OnReceivedMessage += onMessageAdded;
        }

        public async void initAsync()
        {
            Room room = await App.Model.JoinRoom(room_id);

            RoomMsgContainer.Header = room.name;

            List<Message> messages = await App.Model.GetMessages(room_id);

            foreach (var message in messages)
            {
                addMessage(RoomMsgContainer, message);
            }
        }

        private void addMessage(ItemsControl parent, Message message)
        {
            if (parent == null) {
                parent = RoomMsgContainer;
            }

            var item = new TreeViewItem();
            item.Header = message.text + ", " + message.author.name + ", " + message.date;
            parent.Items.Add(item);
            item.Tag = new MessageGUIState( (long) message.id);

            item.MouseDoubleClick += Item_MouseDoubleClick;

            if (parent is TreeViewItem)
            {
                TreeViewItem treeItem = (TreeViewItem)parent;
                treeItem.IsExpanded = true;
            }
        }

        private async void Item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!((TreeViewItem)sender).IsSelected)
            {
                return;
            }

            TreeViewItem item = (TreeViewItem)sender;
            MessageGUIState state = (MessageGUIState)item.Tag;
            Trace.WriteLine(String.Format("open message id {0}", state.message_id));

            if (state.expanded)
            {
                item.IsExpanded = false;
                item.Items.Clear();
                Trace.WriteLine("clear");
            }
            else
            {
                
                item.Items.Clear();
                foreach (var message in await App.Model.GetMessages(room_id, state.message_id))
                {
                    addMessage(item, message);
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            String text = MessageInput.Text;
            MessageInput.Text = "";

            TreeViewItem selected = (TreeViewItem)MessageTree.SelectedItem;
            Message message;

            if (selected == null)
            {
                message = await App.Model.SendMessage(text);
                return;
            }

            MessageGUIState state = (MessageGUIState)selected.Tag;
           
            if (state == null)
            {
                message = await App.Model.SendMessage(text);
                return;
            }

            message = await App.Model.SendMessage(text, state.message_id);
        }
    }

}
