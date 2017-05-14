using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MonkeyHubApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set {
                if(SetProperty(ref _searchTerm, value))
                    SearchCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<string> Resultados { get; }

        public Command SearchCommand { get; }

        public MainViewModel()
        {
            SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
            Resultados = new ObservableCollection<string>() {"abc", "cde" };
        }
       
        async void ExecuteSearchCommand()
        {
            //App.Current.MainPage - não recomendado pois código fica acoplado não permitindo testes
            var resposta = await App.Current.MainPage.DisplayAlert("MonkeyHubApp", $"Você procurou por: '{SearchTerm}'", "Sim", "Não");
            if (resposta)
            {
                await App.Current.MainPage.DisplayAlert("MonkeyHubApp", "Obrigado por pesquisar", "OK");
                Resultados.Add("Sim");
            }
            else
            {
                Resultados.Add("Não");
            }

        }

        bool CanExecuteSearchCommand()
        {
            return !string.IsNullOrWhiteSpace(SearchTerm);
        }

    }
}

/*
private string _descricao;
public string Descricao
{
    get { return _descricao; }
    set
    {
        SetProperty(ref _descricao, value);
        //_descricao = value;
        //OnPropertyChanged();
    }
}

public MainViewModel()
{
    Descricao = "Olá mundo I'm here...";
}
*/