using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections;

namespace ChecklistBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>

        ArrayList checklists = new ArrayList();
        
        

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                if (activity.Text.Equals("start checklist", StringComparison.InvariantCultureIgnoreCase))
                {
                    reply = activity.CreateReply($"Sure dude, let's get a checklist started for you. Type out your checklist as a list of tasks separated by commas.");
                    
                }else if(activity.Text.StartsWith("title:", StringComparison.InvariantCultureIgnoreCase))
                {
                    reply = HandleChecklistCreation(activity);
                }else if(activity.Text.StartsWith("search", StringComparison.InvariantCultureIgnoreCase))
                {
                    reply = HandleChecklistSearch(activity);
                }

                // return our reply to the user
                
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
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

                return message.CreateReply("Hello BotFace!!");
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
                return message.CreateReply("Teehee I know you're typing");
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private Activity HandleChecklistCreation(Activity activity)
        {
            
            String messageText = activity.Text;
            String[] words = messageText.Split(',');
            checklists.Add(words);

            return activity.CreateReply($"You just gave me a checklist and I stored it! Ask for it later :) ");
        }

        private Activity HandleChecklistSearch(Activity activity)
        {
            String[] returnedChecklist = new String[10];
            String[] searchFor = activity.Text.Split(' ');
            String searchItem = searchFor[1];
            System.Diagnostics.Debug.WriteLine(searchItem);

            for(int i = 0; i < checklists.Count; i++)
            {
                String[] tasklist = (String[]) checklists[i];
                System.Diagnostics.Debug.WriteLine(tasklist[0]);
                if (tasklist[0].Contains(searchItem))
                {
                    returnedChecklist = tasklist;
                }
            }
            
            return activity.CreateReply($"I'm returning your checklist now");
        }
    }
}