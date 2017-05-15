using MonkeyHubApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyHubApp.ViewModels
{
    public class ContentWebViewModel : BaseViewModel
    {
        public Content Content { get; }
        public ContentWebViewModel(Content content)
        {
            Content = content;
        }
    }
}
