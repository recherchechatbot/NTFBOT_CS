using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using ServiceStack.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;


namespace Bot_Application
{
    
    //[LuisModel("7bdd8be2-33f1-4be7-9bb8-54e0fe8d15e4", "97b706dcc753412cadc7bb66d615ce1a", domain: "westeurope.api.cognitive.microsoft.com")]
    //[Serializable]
    //public class MyLuisDialog : LuisDialog<object>
    //{
    //    [LuisIntent("listeCourses")]
    //    public async Task listeCourses(IDialogContext context, LuisResult result)
    //    {
    //        await context.PostAsync("La liste de courses procure un gain de temps considérable. Elle te donne la possibilité par un simple clic de déposer dans ton panier les articles que tu commandes régulièrement.Pour que tes prochaines commandes soient plus rapides, tu peux créer des listes thématiques. Remplis ton panier avec les articles désirés, clique ensuite sur « Aller en caisse », puis clique sur le lien « Tout ajouter à une liste ». Donne un nom à ta liste et le tour est joué ! Ta liste de courses est enregistrée, tu pourras la réutiliser lors de ta prochaine visite sur notre site.");
    //    }

    //    [LuisIntent("")]
    //    public async Task None(IDialogContext context, LuisResult result)
    //    {

    //        string message = $"I'm the Notes bot. I can understand requests to create, delete, and read notes. \n\n Detected intent: " + string.Join(", ", result.Intents.Select(i => i.Intent));
    //        await context.PostAsync(message);
    //        context.Wait(MessageReceived);
    //    }
    //}

    [Serializable]
    public class MyBot : IDialog<object>
    {
        
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            //var myToken = "32e88d45-0f1a-4d39-b35b-a8469da5ad10";
            if (message.Text == "recettes")
            {
                PromptDialog.Text(
                    context,
                    ResumeAfterProductDefined,
                    "Qu'est-ce qui te fait envie?",
                    null,
                    0
                    );
            }
            else
            {
                await context.PostAsync("Je n'ai pas compris désolé.");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task ResumeAfterProductDefined(IDialogContext context, IAwaitable<string> argument)
        {
            var product = await argument;
            //var url = $"https://wsmcommerce.intermarche.com/api/v1/recherche/recette?mot=%22{product}%22";
            //var client = new RestClient(url);
            //await context.PostAsync($"{product} 1");
            //var request = new RestRequest(Method.GET);
            //await context.PostAsync($"{product} 2");
            //request.AddHeader("postman-token", "0c334483-3d42-d004-c141-bd5942b728cf");
            //await context.PostAsync($"{product} 3");
            //request.AddHeader("cache-control", "no-cache");
            //await context.PostAsync($"{product} 4");
            //request.AddHeader("TokenAuthentification", "32e88d45-0f1a-4d39-b35b-a8469da5ad10");
            //await context.PostAsync($"{product} 5");
            //IRestResponse response = client.Execute(request);

            //Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" + response.Content);
            //await context.PostAsync($"{product} 6");
            ////JObject jsonResponse = JsonConvert.DeserializeObject(responseContent);
            ////var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString()‌​);
            //await context.PostAsync("Je n'ai pas compris désolé.");
            ////await context.PostAsync(responseContent);
            //await context.PostAsync($"{product}");



            //string url = $"https://wsmcommerce.intermarche.com/api/v1/recherche/recette?mot=%22{product}%22";

            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpWebRequest.Method = WebRequestMethods.Http.Get;
            //httpWebRequest.Headers["tokenauthentification"] = "32e88d45-0f1a-4d39-b35b-a8469da5ad10";
            //httpWebRequest.Accept = "text/json";
            //httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();            
            //var myJSON = JsonConvert.SerializeObject(response);
            //await context.PostAsync(myJSON);
            string url = "https://wsmcommerce.intermarche.com/";

            try
            {
                using (var Client = new HttpClient())
                {
                    Client.BaseAddress = new Uri(url);
                    Client.DefaultRequestHeaders.Accept.Clear();
                    //Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    Client.DefaultRequestHeaders.Add("TokenAuthentification", "32e88d45-0f1a-4d39-b35b-a8469da5ad10");
                    HttpResponseMessage response = await Client.GetAsync($"api/v1/recherche/recette?mot=\"{product}\"");
                    if (response.IsSuccessStatusCode)
                    {
                        var JSON = await response.Content.ReadAsStringAsync();
                        await context.PostAsync(JSON);
                    }
                    else
                    {
                        await context.PostAsync("C'est raté");
                    }

                }
            }
            catch (Exception e)
            {
                await context.PostAsync("Exception");
            }
            context.Wait(MessageReceivedAsync);
        }
    }

    [BotAuthentication]

    public class MessagesController : ApiController
    {
        
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // Check if activity is of type message
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new MyBot());
            }
            else
            {
                this.HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            return response;
           
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

    }
    
    
}