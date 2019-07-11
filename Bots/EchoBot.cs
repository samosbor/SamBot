// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kusto.Cloud.Platform.Utils;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            Bot.Schema.Activity activity = (Bot.Schema.Activity)turnContext.Activity;

            Bot.Schema.Activity reply = activity.CreateReply();
            HeroCard card = new HeroCard
            {
                Title = "Login to Microsoft",
                Text = "Login to access SAM queries",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Login", value: "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47/oauth2/v2.0/authorize?client_id=974c5166-f8b5-466e-9552-306d5f8979e6&scope=https://graph.microsoft.com/.default&response_type=code&redirect_uri=https://10f3f49f.ngrok.io/api/auth/tokenCallback") }
            };
            reply.Attachments.Add(card.ToAttachment());

            await turnContext.SendActivityAsync(reply);
            
            await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);





            /* HttpClient client = new HttpClient();
             client.DefaultRequestHeaders.Add("Authorization", "bearer ");

             var values = new Dictionary<string, string>
             {
                { "thing1", "hello" },
                { "thing2", "world" }
             };

             var content = new FormUrlEncodedContent(values);

             var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

             var responseString = await response.Content.ReadAsStringAsync();*/


            var client = KustoClientFactory.CreateCslQueryProvider("https://aznw.kusto.windows.net/aznwmds;Fed=true");
            var reader = client.ExecuteQuery("TunnelEventsTable | count");


        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }
    }
}
