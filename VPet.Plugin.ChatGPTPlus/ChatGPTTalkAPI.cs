using LinePutScript.Localization.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using VPet_Simulator.Windows.Interface;
using static VPet_Simulator.Core.GraphHelper;

namespace VPet.Plugin.ChatGPTPlugin
{
    public class ChatGPTTalkAPI : TalkBox
    {
        public ChatGPTTalkAPI(ChatGPTPlugin mainPlugin) : base(mainPlugin)
        {
            Plugin = mainPlugin;

            // Add the possible actions to the initialisation message
            // remove everything after Availible actions if it already exist
            if (Plugin.CGPTClient.Completions["vpet"].messages[0].content.Contains("Availible actions"))
            {
                Plugin.CGPTClient.Completions["vpet"].messages[0].content = Plugin.CGPTClient.Completions["vpet"].messages[0].content.Split(new string[] { "Availible actions" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            Plugin.CGPTClient.Completions["vpet"].messages[0].content += "\nAvailible actions : " + string.Join(", ", Get_Works().Select(x => x.Name.Translate().ToLower()));
        }
        protected ChatGPTPlugin Plugin;
        public override string APIName => "ChatGPT";
        public static string[] like_str = new string[] { "陌生", "普通", "喜欢", "爱" };
        public static int like_ts(int like)
        {
            if (like > 50)
            {
                if (like < 100)
                    return 1;
                else if (like < 200)
                    return 2;
                else
                    return 3;
            }
            return 0;
        }
        public override void Responded(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            if (Plugin.CGPTClient == null)
            {
                Plugin.MW.Main.SayRnd("请先前往设置中设置 ChatGPT API".Translate());
                return;
            }
            Dispatcher.Invoke(() => this.IsEnabled = false);
            try
            {
                if (Plugin.CGPTClient.Completions.TryGetValue("vpet", out var vpetapi))
                {
                    while (vpetapi.messages.Count > Plugin.KeepHistory + 1)
                    {
                        vpetapi.messages.RemoveAt(1);
                    }
                    var last = vpetapi.messages.LastOrDefault();
                    if (last != null)
                    {
                        if (last.role == ChatGPT.API.Framework.Message.RoleType.user)
                        {
                            vpetapi.messages.Remove(last);
                        }
                    }
                }
                var parameters = new Dictionary<string, string>()
                {
                    { "currentMode", Plugin.MW.Core.Save.Mode.ToString().Translate() },
                    { "likabilityText", like_str[like_ts((int)Plugin.MW.Core.Save.Likability)].Translate() },
                    { "money", Plugin.MW.Core.Save.Money.ToString() },
                    { "level",  Plugin.MW.Core.Save.Level.ToString() },
                    { "health", Math.Round(Plugin.MW.Core.Save.Health).ToString() + "/100"},
                    { "starmina", Math.Round(Plugin.MW.Core.Save.Strength).ToString() + "/100"},
                    { "food", Math.Round(Plugin.MW.Core.Save.StrengthFood).ToString() + "/100"},
                    { "drink", Math.Round(Plugin.MW.Core.Save.StrengthDrink).ToString() + "/100"},
                    { "feeling", Math.Round(Plugin.MW.Core.Save.Feeling).ToString() + "/100" }
                };
                string statusText = "Assistant status = [";
                foreach (var item in parameters)
                {
                    statusText += item.Key + ":" + item.Value + ",";
                }
                statusText = statusText.TrimEnd(',') + "]\r\n";

                content = statusText + content;

                var resp = Plugin.CGPTClient.Ask("vpet", content);
                var reply = resp.GetMessageContent();
                reply = Execute_Actions(reply);
                if (resp.choices[0].finish_reason == "length")
                {
                    reply += " ...";
                }
                var showtxt = Plugin.ShowToken ? null : "当前Token使用".Translate() + ": " + resp.usage.total_tokens;
                Plugin.MW.Main.SayRnd(reply, desc: showtxt);
            }
            catch (Exception exp)
            {
                var e = exp.ToString();
                string str = "请检查设置和网络连接".Translate();
                if (e.Contains("401"))
                {
                    str = "请检查API token设置".Translate();
                }
                Plugin.MW.Main.SayRnd("API调用失败".Translate() + $",{str}\n{e}");//, GraphCore.Helper.SayType.Serious);
            }
            Dispatcher.Invoke(() => this.IsEnabled = true);
        }

        /// <summary>
        /// Edit the stats of the pet depending on the reply
        /// for exemple if the reply contain {starmina+10} we call Add_Stat("starmina+10")
        /// </summary>
        /// <param name="element"></param>
        /// <returns>the reply with the actions</returns>
        private string Execute_Actions(string reply)
        {
            var actions = reply.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var action in actions)
            {
                if (action.Contains("+"))
                {
                    Add_Stat(action);
                }
                else if (action.Contains("-"))
                {
                    Remove_Stat(action);
                }
                else if (action.Contains("go:"))
                {
                    Go_To(action);
                }
                else
                {
                    continue;
                }
                reply = reply.Replace("{" + action + "}", "");
            }
            return reply;
        }

        /// <summary>
        /// starmina+10 we add 10 to Plugin.MW.Core.Save.Strength
        /// </summary>
        /// <param name="action"></param>
        private void Add_Stat(string action)
        {
            action = action.Replace(" ", "");
            var stat = action.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (stat.Length != 2)
            {
                return;
            }
            var stat_name = stat[0];
            var stat_value = stat[1];
            if (stat_name == "starmina")
            {
                Plugin.MW.Core.Save.Strength += int.Parse(stat_value);
            }
            else if (stat_name == "food")
            {
                Plugin.MW.Core.Save.StrengthFood += int.Parse(stat_value);
            }
            else if (stat_name == "drink")
            {
                Plugin.MW.Core.Save.StrengthDrink += int.Parse(stat_value);
            }
            else if (stat_name == "health")
            {
                Plugin.MW.Core.Save.Health += int.Parse(stat_value);
            }
            else if (stat_name == "feeling")
            {
                Plugin.MW.Core.Save.Feeling += int.Parse(stat_value);
            }
            else if (stat_name == "money")
            {
                Plugin.MW.Core.Save.Money += int.Parse(stat_value);
            }
        }

        /// <summary>
        /// starmina-10 we remove 10 to Plugin.MW.Core.Save.Strength
        /// </summary>
        /// <param name="action"></param>
        private void Remove_Stat(string action)
        {
            action = action.Replace(" ", "");
            var stat = action.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (stat.Length != 2)
            {
                return;
            }
            var stat_name = stat[0];
            var stat_value = stat[1];
            if (stat_name == "starmina")
            {
                Plugin.MW.Core.Save.Strength -= int.Parse(stat_value);
            }
            else if (stat_name == "food")
            {
                Plugin.MW.Core.Save.StrengthFood -= int.Parse(stat_value);
            }
            else if (stat_name == "drink")
            {
                Plugin.MW.Core.Save.StrengthDrink -= int.Parse(stat_value);
            }
            else if (stat_name == "health")
            {
                Plugin.MW.Core.Save.Health -= int.Parse(stat_value);
            }
            else if (stat_name == "feeling")
            {
                Plugin.MW.Core.Save.Feeling -= int.Parse(stat_value);
            }
            else if (stat_name == "money")
            {
                Plugin.MW.Core.Save.Money -= int.Parse(stat_value);
            }

        }

        /// <summary>
        /// go:Gaming we call StartWork(Gaming)
        /// </summary>
        /// <param name="action"></param>
        private void Go_To (string action)
        {
            action = action.Replace("go:", "").ToLower();
            if (Get_Works().Any(x => x.Name.Translate().ToLower() == action))
            {
                StartWork(Get_Works().First(x => x.Name.Translate().ToLower() == action));
            }
        }
        private List<Work> Get_Works()
        {
            return Plugin.MW.Core.Graph.GraphConfig.Works;
        }


        /// <summary>
        /// call the StartWork function from Toolbar.xaml.cs
        /// </summary>
        /// <param name="work"></param>
        private void StartWork(Work work)
        {
            // Plugin.MW.Main.ToolBar.StartWork(work); // STA error
            Dispatcher.Invoke(() => Plugin.MW.Main.ToolBar.StartWork(work));
        }

        public override void Setting() => Plugin.Setting();
    }
}
