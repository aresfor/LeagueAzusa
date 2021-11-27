namespace LeagueAzusa
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    //FIXME 已知问题，setforewindow的时候模拟输入enter无效
    //粘贴板不共享无法使用ctrl+v，只能通过搜狗输入法设置短语，每次游戏开局前要手动打开配置文件让程序输入
    //以上就是输入和焦点问题
    public partial class LeagueAzusaWindow : Form
    {
        readonly HttpClient client;
        //int为EventID
        public Dictionary<int, EventMsg> events;
        //string均为召唤师名字
        public Dictionary<string, Player> players;
        public Dictionary<string, Player> teammates;
        public Dictionary<string, Player> enemys;
        //第一个string为召唤师名字，第二个为在快捷短语中代表输入的快捷
        public Dictionary<string, string> sogouDic;
        //本地玩家，游戏开始Init时获取
        public string activePlayerName = "";
        //"LeagueClient";
        static string clientName = "League of legends";
        //只有打开配置文件的时候这个文件才会出现，关闭之后会被写入数据文件中
        static string filePath = @"C:\Users\Administrator\AppData\LocalLow\SogouPY\PhraseEdit.txt";
        bool isGameStarted = false;

        #region Win32API获取窗口
        [DllImport("USER32.DLL")]
        static extern int SetForegroundWindow(IntPtr point);
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(IntPtr point);
        //(string lpClassName ,string lpWindowName);
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
    string lpWindowName);
        #endregion
        public LeagueAzusaWindow()
        {
            InitializeComponent();
            HttpClientHandler clientHandler = new HttpClientHandler();
            events = new Dictionary<int, EventMsg>();
            players = new Dictionary<string, Player>();
            teammates = new Dictionary<string, Player>(); 
            enemys = new Dictionary<string,Player>();
            sogouDic = new Dictionary<string, string>();

            //略过证书验证
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
            UriBuilder uriBuilder = new UriBuilder("https", "127.0.0.1", 2999);
            client.BaseAddress = uriBuilder.Uri;
            
        }
        /// <summary>
        /// 游戏开始时调用
        /// </summary>
        private async void Init()
        {
            activePlayerName = await GetLiveData("activeplayername");
            //json->string
            activePlayerName = activePlayerName.Replace("\\", string.Empty).Replace("\"", string.Empty);
            //EventTimer.Enabled = true;
            await GetPlayerList();
            //将所有玩家加入dic便于输入法使用快捷短语
            using (FileStream f = new FileStream(filePath, FileMode.Open))
            {
                using (StreamWriter fs = new StreamWriter(f, Encoding.Unicode))
                {
                    string i = "p";
                    foreach (var p in enemys.Values)
                    {
                        fs.WriteLine($"{i},1={p.SummonerName}");
                        sogouDic.Add(p.SummonerName,i);
                        i += "p";
                    }
                }
            }
        }
        /// <summary>
        /// Tick获取游戏事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTimer_Tick(object sender, EventArgs e)
        {
            GetLiveEvent();
        }
        /// <summary>
        /// 获取运行时事件数据
        /// </summary>
        async void GetLiveEvent()
        {
            
            string res = await GetLiveData("eventdata");
            List<EventMsg> msgs = AzusaDeserialize<EventMsg>(res, "Events");

            foreach (var e in msgs)
            {
                if(!events.ContainsKey(e.EventId))
                {
                    events.Add(e.EventId, e);
                    //if(e.EventName == "GameStart")
                    if(e.EventId == 0)
                    {
                        Init();
                    }
                    if(e.EventName == "ChampionKill")
                    {
                        //if(teammates.ContainsKey(e.VictimName) || (e.Assisters!=null && e.Assisters.Count == 0))
                        //{
                            string content = $"{e.VictimName} 竟然被单杀，成为下等马";
                            InputToClient(content, e.VictimName);
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// 将内容输入到客户端，实现具有问题
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="player">对象，未具体</param>
        void InputToClient(string content,string player)
        {
            Process? p = Process.GetProcessesByName(clientName).FirstOrDefault();
            richTextBox1.Text = content;
            //Clipboard.SetText(content);
            
            if (p != null)
            {
                richTextBox1.Text += "WindowFInded";
                IntPtr h = p.MainWindowHandle;
                //IntPtr window = FindWindow(null,"League of Legends（TM）Client");
                //p.Kill();
                SetForegroundWindow(h);

                Thread.Sleep(2000);
                if (!sogouDic.ContainsKey(player))
                    return;
                string playerSogouId = sogouDic[player];
                SendKeys.SendWait(playerSogouId);
                SendKeys.SendWait("1");
                Thread.Sleep(200);

                SendKeys.SendWait("ds");

                SendKeys.SendWait("1");
                Thread.Sleep(200);

                SendKeys.SendWait("{ENTER}");
            }
            

        }
        public async Task GetPlayerList()
        {
            string res = await GetLiveData("playerlist");

            List<Player> msgs = AzusaDeserializeArray<Player>(res);
            foreach (var m in msgs)
            {
                if (!players.ContainsKey(m.SummonerName))
                {
                    players.Add(m.SummonerName, m);
                }
                else
                {
                    players[m.SummonerName] = m;
                }
            }
            foreach (var p in players.Values)
            {
                if (!players.ContainsKey(activePlayerName))
                {
                    MessageBox.Show("获取的玩家中没有本地玩家，检查本地玩家或者获取玩家是否出错");
                    return;
                }
                string teamFlag = players[activePlayerName].Team;
                if (p.Team == teamFlag)
                {
                    teammates.Add(p.SummonerName, p);
                }
                else
                {
                    enemys.Add(p.SummonerName, p);
                }
            }
            foreach (var item in players)
            {
                richTextBox1.Text += item.Value.SummonerName + item.Value.ChampionName + "\n";
            }
        }
        #region 测试函数
        void testWrite()
        {
            using (FileStream f = new FileStream(filePath, FileMode.Open))
            {
                using (StreamWriter fs = new StreamWriter(f, Encoding.Unicode))
                {
                    fs.WriteLine("dh,1=" + "fghfg");
                }
            }
            //File.WriteAllLines(filePath, c);
        }
        public async void testEvent()
        {
            string res = await GetLiveData("eventdata");
            List<EventMsg> msgs = AzusaDeserialize<EventMsg>(res, "eventdata");
            if (msgs == null)
            { return; }
            string myText = "";
            foreach (var item in msgs)
            {
                myText += item.EventId + "-" + item.EventName + "-" + (int)item.EventTime + "-" + item.KillerName;
                myText += "\n";
            }
            richTextBox1.Text = myText;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            //testWrite();
            //GetLiveEvent();
            EventTimer.Enabled = true;
            isGameStarted = true;
            //test();

            //MessageBox.Show("hi");
            //SetForegroundWindow
            //richTextBox1.Text = "你好世界";
            //richTextBox1.Focus();
            //richTextBox1.SelectAll();
            //SendKeys.SendWait("^C");
            //SendKeys.SendWait("A");
            //SendKeys.SendWait("^V");
        }

        
        /// <summary>
        /// 根据Riot API文档的路由名称来获取运行时json数据
        /// </summary>
        /// <param name="dataRoute">数据路由字段,例如eventdata</param>
        /// <returns></returns>
        public async Task<string> GetLiveData(string dataRoute)
        {
            if (!isGameStarted) { return ""; }
            string liveRoute = "liveclientdata";
            return await GetData(liveRoute, dataRoute);
        }
        /// <summary>
        /// 序列化Riot API返回的数组，例如playerlist
        /// </summary>
        /// <typeparam name="T">接受数据的类</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public List<T> AzusaDeserializeArray<T>(string json)
        {
            List<T>? results = new List<T>();
            if(json == null || json.Length == 0)
            { return results; }

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //MyJsonConverter<Runes> i = new MyJsonConverter<Runes>();
            //settings.Converters.Add(i);
            results = JsonConvert.DeserializeObject<List<T>>(json);
            return results;
        }
        /// <summary>
        /// 反序列化Riot API返回的json数据
        /// </summary>
        /// <typeparam name="T">保存返回数据的类</typeparam>
        /// <param name="results">返回结果</param>
        /// <param name="json"></param>
        /// <param name="routes">节点路由</param>
        public List<T> AzusaDeserialize<T>( string json, params string[] routes)
        {
            List<T> results = new List<T>();
            if(json == null || json.Length == 0)
            { return results; }
            //if (routes.Length == 0) { return results; }
            

            JObject root = JObject.Parse(json);
            JToken? partRoot = root;
            foreach (var r in routes)
            {
                if (r == null || root[r] == null)
                {
                    results.Clear();
                    return results;
                }
                partRoot = partRoot[r];
            }
            IList<JToken> tokens = partRoot?.Children().ToList() ?? new List<JToken>();
            foreach (var token in tokens)
            {
                T? res = token.ToObject<T>();
                if (res != null)
                {
                    results.Add(res);
                }
            }
            return results;

        }
        async Task<string> GetData(params string[] routes)
        {
            string route = "";
            foreach (string r in routes)
            {
                route += r + '/';
            }
            route = route.Remove(route.Length - 1);
            string result = "";
            try
            {
                result = await client.GetStringAsync(route);
            }
            catch (Exception ex)
            {
                if (!isGameStarted) { return ""; }
                if (ex.Message.Contains("404"))
                    MessageBox.Show("未进入游戏或已经断开连接");
                else
                {
                    EventTimer.Enabled = false;
                    isGameStarted = false;
                    MessageBox.Show(ex.Message);
                }
            }
            
            return result;
        }

    }
}