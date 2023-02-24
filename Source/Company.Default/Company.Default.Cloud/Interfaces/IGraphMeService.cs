namespace Company.Default.Cloud.Interfaces
{
    public interface IGraphMeService
    {
        /// <summary>
        /// Calls graph's endpoint /me
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/graph/api/resources/users?view=graph-rest-1.0">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <param name="properties"></param>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a dynamic object.
        /// <para>
        /// If the properties parameter is null, it will return the <see cref="Microsoft.Graph.User"/>, 
        /// otherwise it will only return the requested properties.
        /// </para>
        /// </returns>
        Task<dynamic> GetAsync(string[] properties = null);

        /// <summary>
        /// Calls graph's endpoint /me/photo.
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/graph/api/resources/profilephoto?view=graph-rest-1.0">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a <see cref="Stream"/>
        /// </returns>
        Task<Stream> GetProfilePhotoAsStream();

        /// <summary>
        /// Calls graph's endpoint /me/photo
        /// <para>
        /// <see href="https://learn.microsoft.com/en-us/graph/api/resources/profilephoto?view=graph-rest-1.0">Go to Microsoft Learn</see>
        /// </para>
        /// </summary>
        /// <returns>
        /// <see cref="Task{TResult}"/> where TResult is a encoded base64 <see cref="string"/>
        /// </returns>
        Task<string> GetProfilePhotoAsBase64();
    }
}