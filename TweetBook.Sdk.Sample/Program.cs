using Refit;
using System;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Requests;

namespace TweetBook.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");

            //var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            //{
            //    Email = "sdkaccount@test.com",
            //    Password = "Password123!"
            //});

            //await identityApi.AddRoleToUserAsync(new AddRoleToUserRequest
            //{
            //    UserEmail = "sdkaccount@test.com",
            //    RoleName = "Admin"
            //});

            //await identityApi.AddRoleToUserAsync(new AddRoleToUserRequest
            //{
            //    UserEmail = "sdkaccount@test.com",
            //    RoleName = "Poster"
            //});

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "worker@chapsas.com",
                Password = "Password123!"
            });

            cachedToken = loginResponse.Content.Token;

            var tweetBookApi = RestService.For<ITweetBookApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var allPosts = await tweetBookApi.GetAllAsync();

            foreach(var post in allPosts.Content)
            {
                Console.WriteLine($"Post {post.Name} by user {post.UserId}!");
            }

            Console.WriteLine();

            var createdPost = await tweetBookApi.CreatePostAsync(new CreatePostRequest
            {
                Name = "New post by SDK"
            });

            Console.WriteLine($"Created post {createdPost.Content.Name} by user {createdPost.Content.UserId}!");

            var retrievedPost = await tweetBookApi.GetPostAsync(createdPost.Content.Id);

            Console.WriteLine($"Get created post {retrievedPost.Content.Name} by user {retrievedPost.Content.UserId}!");

            var updatedPost = await tweetBookApi.UpdatePostAsync(createdPost.Content.Id, new UpdatePostRequest 
            {
                Name = "Updated post by SDK"
            });

            Console.WriteLine($"Updated post {updatedPost.Content.Name} by user {updatedPost.Content.UserId}!");

            var deletedPost = await tweetBookApi.DeletePostAsync(createdPost.Content.Id);

            Console.WriteLine($"Delete post {updatedPost.Content.Name} by user {updatedPost.Content.UserId}!");

            Console.ReadLine();
        }
    }
}
