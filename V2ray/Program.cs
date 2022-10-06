
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using V2ray.Base;
using V2ray.Model;

string input = string.Empty;

if (!Directory.Exists(AppContext.BaseDirectory + MainConstants.FolderPath))
{
    Directory.CreateDirectory(AppContext.BaseDirectory + MainConstants.FolderPath);
}
try
{
    var appConfiguration = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath));
}
catch (Exception e)
{
    File.WriteAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath, JsonConvert.SerializeObject(new Config()));
}

var appConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath));

#if DEBUG
var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(AppContext.BaseDirectory + "config.json"));
#endif
#if !DEBUG
var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(MainConstants.ConfgPath));
#endif

if (config is null)
{
    Console.WriteLine("Error: V2ray not installed !");

    return;
}

string Base64Encode(string plainText)
{
    byte[] bytes = Encoding.UTF8.GetBytes(plainText);
    return Convert.ToBase64String(bytes);
}

string Base64Decode(string base64EncodedData)
{
    byte[] bytes = Convert.FromBase64String(base64EncodedData);
    return Encoding.UTF8.GetString(bytes);
}

void saveToConfig()
{
#if DEBUG
    using (StreamWriter file = File.CreateText(AppContext.BaseDirectory + "config.json"))
#endif
#if !DEBUG
using (StreamWriter file = File.CreateText(MainConstants.ConfgPath))

#endif
    {
        file.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
    }
}

menu:

var startInfo = new ProcessStartInfo()
{
    CreateNoWindow = true,
    RedirectStandardError = true,
    RedirectStandardOutput = true,
    RedirectStandardInput = true,
    FileName = "/bin/bash",
    Arguments = $"-c \"sudo systemctl restart v2ray.service\""
};

Process proc = new Process() { StartInfo = startInfo, };

proc.Start();

Console.ForegroundColor = ConsoleColor.Blue;

Console.Out.Flush();

Console.Clear();

Console.WriteLine("=================== Hello Welcome to V2ray Manager ===================");

Console.Write($"\n#####\t1. Get User  \t\t2. Get Users\t#####");

Console.Write($"\n#####\t3. Create New User \t\t4. Delete User\t#####");

Console.Write($"\n#####\t5. Manual Settings \t\t6. Update Config\t#####\n");

input = Console.ReadLine();

Console.ForegroundColor = ConsoleColor.Green;

goto decide;


updateConfig:
return;

getUsers:
return;

createNewUser:

Console.Write("Type UserName: ");

input = Console.ReadLine();

var model = new Client();
model.alterId = 0;
model.createDate = DateTime.Now.ToString("yyyy-MM-dd");
model.level = Convert.ToInt16(appConfig.Level);
model.id = Guid.NewGuid().ToString();
model.username = input;

if ((bool)config.inbounds[0].settings.Clients.Any(m => m.username == input))
{
    Console.WriteLine("Error: Username is exist !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

config?.inbounds[0]?.settings.Clients.Add(model);
//convert to json and add to clients VPN config

saveToConfig();

Console.Clear();

foreach (var hostName in appConfig.HostNames)
{//new user based on config

    var user = new User
    {
        Ps = appConfig.Name,
        Port = appConfig.Port,
        Net = appConfig.Net,
        Type = appConfig.Type,
        Add = hostName,
        Id = model.id,
        Aid = "0",
        V = appConfig.Level
    };
    var userJson = JsonConvert.SerializeObject(user, Formatting.Indented);

    //print each one
    Console.WriteLine($"\nvmess://{Base64Encode(userJson)}\n");
}

Console.WriteLine("User created successfuly !\nPress a key to continue ...");

Console.ReadLine();

goto menu;

deleteUser:
Console.Clear();

Console.WriteLine("Please enter Id or enter Username");

input = Console.ReadLine();

var data = config.inbounds[0].settings.Clients.Where(m => m.username == input || m.id == input).FirstOrDefault();

if (data is null)
{
    Console.WriteLine("Error: Username is not exist !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

input = Console.ReadLine();

if (input.ToLower() != "y")
{
    Console.WriteLine("Operation canceled by user !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

config.inbounds[0].settings.Clients.Remove(data);

saveToConfig();

Console.WriteLine("User deleted successfuly !\nPress a key to continue ...");

Console.ReadKey();

goto menu;

manualSettings:
goto menu;


decide:
switch (Convert.ToInt32(input))
{
    case 1:
        goto updateConfig;

    case 2:
        goto getUsers;

    case 3:
        goto createNewUser;

    case 4:
        goto deleteUser;

    case 5:
        goto manualSettings;

    case 6:
        goto deleteUser;

    default:
        break;
}


Console.ReadKey();


//using Newtonsoft.Json;
//using V2ray.Model;



//Console.WriteLine(JsonConvert.SerializeObject(new Config()
//{
//    HostNames = new List<string>()
//    {
//        "de.leastpng.pw",
//        "auto.leastpng.pw"
//    },
//    Name = "Server DE",
//    Net = "ws",
//    Port = "25854",
//    Type = "none",
//    Level = "1"
//}));

//Console.ReadLine();