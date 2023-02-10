using Company.Default.Cloud.Interfaces;
using Microsoft.Graph;
using System.Dynamic;
using System.Reflection;

namespace Company.Default.Cloud.Graph
{
    public class GraphMeService : IGraphMeService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GraphMeService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<dynamic> GetAsync(string[] properties = null)
        {
            if (properties == null)
                return await _graphServiceClient.Me.Request().GetAsync();

            var select = string.Join(',', properties);

            var user = await _graphServiceClient.Me.Request().Select(select).GetAsync();
            var result = GetDynamicWithSelectedProperties(user, properties);

            return result;
        }

        public async Task<Stream> GetProfilePhotoAsStream()
        {
            var photo = await GetPhoto();

            return photo;
        }

        public async Task<string> GetProfilePhotoAsBase64()
        {
            var photo = await GetPhoto();

            using(var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);

                var byteArray = memoryStream.ToArray();

                var base64 = Convert.ToBase64String(byteArray);

                return $"data:image/jpeg;base64, {base64}";
            }
        }

        private async Task<Stream> GetPhoto() =>
            await _graphServiceClient.Me.Photo.Content.Request().GetAsync();

        private dynamic GetDynamicWithSelectedProperties(User user, string[] selectedProperties)
        {
            var dict = new Dictionary<string, object>();

            foreach(var propertyName in selectedProperties)
            {
                var propertyInfo = user.GetType().GetProperty(propertyName,
                       BindingFlags.Instance |
                       BindingFlags.Public |
                       BindingFlags.IgnoreCase);

                if (propertyInfo == null) continue;

                var propertyValue = propertyInfo?.GetValue(user, null);

                dict.Add(propertyName, propertyValue);
            }

            var expandoObj = new ExpandoObject();
            var expanded = (ICollection<KeyValuePair<string, object>>) expandoObj;

            foreach(var kvp in dict)
                expanded.Add(kvp);

            dynamic result = expanded;

            return result;            
        }
    }
}
