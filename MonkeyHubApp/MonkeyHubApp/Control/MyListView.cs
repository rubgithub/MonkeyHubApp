using System.Windows.Input;
using Xamarin.Forms;

namespace MonkeyHubApp.Control
{
    
    //TODO: mudar para static conforme: http://stackoverflow.com/questions/26040652/binding-to-listview-item-tapped-property-from-view-model
    
    //Attached Behaviors LEMBRE-SE
    //https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/behaviors/attached/
    public class MyListView : ListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty =
        BindableProperty.CreateAttached(
            "ItemTappedCommand",
            typeof(ICommand),
            typeof(MyListView),
            null,
            propertyChanged: OnCommandChanged);

        public MyListView(ListViewCachingStrategy strategy) : base(strategy)
        {
        }

        public MyListView() : this(ListViewCachingStrategy.RecycleElement)
        {
        }

        public ICommand ItemTappedCommand {
            get { return (ICommand)GetValue(ItemTappedCommandProperty); }
            set
            {
                SetValue(ItemTappedCommandProperty, value);
            }
        }

        static void OnCommandChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as ListView;
            if (entry == null)
                return;

            entry.ItemTapped += (sender, e) =>
            {
                var command = (newValue as ICommand);
                if (command == null)
                    return;

                if (command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }

            };
        }

    }
}
