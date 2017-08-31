using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace GoogleLocationPrototype.Dialogs
{
    [LuisModel("fd6a04c9-f7f0-4379-9e53-858baad099c3", "06828f1f9b3544a4aff30ba86b317751", domain: "westcentralus.api.cognitive.microsoft.com")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        [LuisIntent("Greetings")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello...!");
            context.Wait(MessageReceived);
        }
        [LuisIntent("FindLocation")]
        public async Task FindLocation(IDialogContext context, LuisResult result)
        {
            string Lname = "";
            EntityRecommendation er;


            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            if (result.TryFindEntity ("LocName",out er))
            {
                Lname = er.Entity;

                List<CardImage> images = new List<CardImage>();
                CardImage ci = new CardImage("https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=AIzaSyAUT0zYwpPk82Y7BsCL1_VSM5Sohk8FEYQ&center=" + Lname);
                images.Add(ci);
                CardAction ca = new CardAction()
                {
                    Title = "Welcome",
                    Type = "openUrl",
                    Value = "https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=600x300&maptype=roadmap&markers=color:blue%7Clabel:S%7C11211%7C11206%7C11222&key=AIzaSyAUT0zYwpPk82Y7BsCL1_VSM5Sohk8FEYQ&center=" + Lname
                };

                ThumbnailCard tc = new ThumbnailCard()
                {
                    Images = images,
                    Tap = ca
                };
                reply.Attachments.Add(tc.ToAttachment());
                await context.PostAsync(reply);
            }

            else
            {
                await context.PostAsync($"Sorry I couldn't find {Lname} for you...!");
            }
            
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have no idea what you are talking about...");
            context.Wait(MessageReceived);
        }
    }
}