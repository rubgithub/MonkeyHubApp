using MonkeyHubApp.Model;
using MonkeyHubApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonkeyHubApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMonkeyHubApiService _monkeyHubApiService;

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set {
                if(SetProperty(ref _searchTerm, value))
                    SearchCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Tag> Resultados { get; }

        public Command SearchCommand { get; }
        public Command AboutCommand { get; }
        public Command ShowCategoryCommand { get; }

        public MainViewModel(IMonkeyHubApiService monkeyHubApiService)
        {
            _monkeyHubApiService = monkeyHubApiService;

            SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
            AboutCommand = new Command(ExecuteAboutCommand);
            ShowCategoryCommand = new Command<Tag>(ExecuteShowCategoryCommand);

            Resultados = new ObservableCollection<Tag>();

        }

        private async void ExecuteShowCategoryCommand(Tag tag)
        {
            await PushAsync<CategoriaViewModel>(_monkeyHubApiService, tag);
        }

        private async void ExecuteAboutCommand()
        {
            await PushAsync<AboutVeiwModel>();
        }

        async void ExecuteSearchCommand()
        {
            //App.Current.MainPage - não recomendado pois código fica acoplado não permitindo testes
            var resposta = await App.Current.MainPage.DisplayAlert("MonkeyHubApp", $"Você procurou por: '{SearchTerm}'", "Sim", "Não");
            if (resposta)
            {
                await App.Current.MainPage.DisplayAlert("MonkeyHubApp", "Obrigado por pesquisar", "OK");

                Resultados.Clear();
                var tagsRetornadas = await _monkeyHubApiService.GetTagsAsync();
                if (tagsRetornadas != null)
                {
                    foreach (var tag in tagsRetornadas)
                    {
                        Resultados.Add(tag);
                    }
                }
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