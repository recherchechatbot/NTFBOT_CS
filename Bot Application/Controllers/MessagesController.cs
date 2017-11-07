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


    //[Serializable]
    //public class EchoDialog : IDialog<object>
    //{
    //    protected int count = 1;
    //    public async Task StartAsync(IDialogContext context)
    //    {
    //        context.Wait(MessageReceivedAsync);
    //    }

    //    public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    //    {
    //        var message = await argument;
    //        if (message.Text == "reset")
    //        {
    //            PromptDialog.Confirm(
    //                context,
    //                AfterResetAsync,
    //                "Ês-tu sûr de vouloir remettre le compteur à 0?",
    //                "Pas compris",
    //                promptStyle: PromptStyle.None);
    //        }
    //        else
    //        {
    //            await context.PostAsync($"{this.count++}: Tu as dit: " + message.Text);
    //            context.Wait(MessageReceivedAsync);
    //        }

    //    }
    //    public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
    //    {
    //        var confirm = await argument;
    //        if (confirm)
    //        {
    //            this.count = 1;
    //            await context.PostAsync("Compteur à 0");
    //        }
    //        else
    //        {
    //            await context.PostAsync("Compteur intact");
    //        }
    //        context.Wait(MessageReceivedAsync);
    //    }
    //}

    [Serializable]
    public class MyBot : IDialog<object>
    {
        protected int count = 1;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            if (message.Text == "recette poulet")
            {
                await context.PostAsync("Ici on mettra les resultats de la recherche de recettes");
                context.Wait(MessageReceivedAsync);
            }
            else if (message.Text=="courses poulet")
            {
                await context.PostAsync("Ici on mettra les resultats de la recherche de produits");
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync("Je n'ai pas compris désolé.");
                context.Wait(MessageReceivedAsync);
            }
           

        }
       

        //public Task StartAsync(IDialogContext context)
        //{
        //    throw new NotImplementedException();
        //}
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