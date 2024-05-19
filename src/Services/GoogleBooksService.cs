using Biblioconecta.Models.Google;
using System.Net.Http.Json;

namespace Biblioconecta.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Book?> GetAsync(string isbn)
        {
            string requestUrl = $"https://www.googleapis.com/books/v1/volumes?key=AIzaSyBpe4rSwGb7IHHyIvSdeea6jG55f1llRAs&q=isbn:{isbn}";
            try
            {
                var response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var bookResponse = await response.Content.ReadFromJsonAsync<BooksResponse>();

                    if (bookResponse != null && bookResponse.Items.Count > 0)
                    {
                        Book? book = bookResponse.Items
                            .Where(e => !string.IsNullOrEmpty(e.VolumeInfo?.ImageLinks?.Thumbnail) || !string.IsNullOrEmpty(e.VolumeInfo?.ImageLinks?.SmallThumbnail))
                            .FirstOrDefault() ?? bookResponse.Items.FirstOrDefault();
                        return book;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }
    }
}
