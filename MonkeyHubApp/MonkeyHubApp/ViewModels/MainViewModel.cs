using MonkeyHubApp.Model;
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
        private const string BaseUrl = "https://monkey-hub-api.azurewebsites.net/api/";

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

        public MainViewModel()
        {
            SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);
            Resultados = new ObservableCollection<Tag>();
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"{BaseUrl}Tags").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<Tag>>(
                        await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }

            return null;
        }

        async void ExecuteSearchCommand()
        {
            //App.Current.MainPage - não recomendado pois código fica acoplado não permitindo testes
            var resposta = await App.Current.MainPage.DisplayAlert("MonkeyHubApp", $"Você procurou por: '{SearchTerm}'", "Sim", "Não");
            if (resposta)
            {
                await App.Current.MainPage.DisplayAlert("MonkeyHubApp", "Obrigado por pesquisar", "OK");

                Resultados.Clear();
                var tagsRetornadas = await GetTagsAsync();
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