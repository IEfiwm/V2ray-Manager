using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using V2ray.Base;
using V2ray.Model;

string input = string.Empty;

var appConfig = new Config();

if (!Directory.Exists(AppContext.BaseDirectory + MainConstants.FolderPath))
{
    Directory.CreateDirectory(AppContext.BaseDirectory + MainConstants.FolderPath);
}

#if DEBUG
var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(AppContext.BaseDirectory + "config.json"));
#endif
#if !DEBUG
var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(MainConstants.ConfgPath));
#endif

if (!File.Exists(AppContext.BaseDirectory + MainConstants.AppConfigPath))
    goto manualSettings;

appConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath));

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
#if !DEBUG
proc.Start();
#endif

Console.ForegroundColor = ConsoleColor.Blue;

Console.Out.Flush();

Console.Clear();

Console.WriteLine("=================== (Hello Welcome to V2ray Manager) ===================");
Console.WriteLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version);

Console.Write($"\n#####\t1. Get User  \t\t\t2. Get All Users\t#####");

Console.Write($"\n#####\t3. Create New User \t\t4. Delete User\t\t#####");

Console.Write($"\n#####\t5. Manual Settings \t\t6. Update Config\t#####\n");

Console.WriteLine("Press '0' for exit !");

input = Console.ReadLine();

Console.ForegroundColor = ConsoleColor.Green;

goto decide;

getUser:

Console.Clear();

Console.WriteLine("Please enter Username or Id");

input = Console.ReadLine();

var mmodel = config?.inbounds[0]?.settings.Clients
    .Where(m => m.id == input || m.username == input)
    .FirstOrDefault();

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
        Id = mmodel.id,
        Aid = "0",
        V = appConfig.Level
    };
    var userJson = JsonConvert.SerializeObject(user, Formatting.Indented);

    //print each one
    Console.WriteLine($"\nvmess://{Base64Encode(userJson)}\n");
}

Console.WriteLine("\nPress a key to continue ...");

Console.ReadKey();

goto menu;

updateConfig:
return;

getUsers:

Console.Clear();

var users = config?.inbounds[0]?.settings.Clients
    .ToList();

foreach (var item in users)
{
    Console.WriteLine("========================================================");

    Console.WriteLine($@"Id: {item.id}");

    Console.WriteLine($@"Username: {item.username}");

    Console.WriteLine($@"CreateDate: {item.createDate}");

    foreach (var hostName in appConfig.HostNames)
    {//new user based on config

        var user = new User
        {
            Ps = appConfig.Name,
            Port = appConfig.Port,
            Net = appConfig.Net,
            Type = appConfig.Type,
            Add = hostName,
            Id = item.id,
            Aid = "0",
            V = appConfig.Level
        };
        var userJson = JsonConvert.SerializeObject(user, Formatting.Indented);

        //print each one
        Console.WriteLine($"\nvmess://{Base64Encode(userJson)}\n");
    }
}
Console.WriteLine("========================================================");

Console.WriteLine($"\nAllUserCount: {users.Count}");

Console.WriteLine("\nPress a key to continue ...");

Console.ReadKey();

goto menu;

createNewUser:

Console.Write("Please type Username: ");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = DateTime.Now.Ticks.ToString();
}

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

Console.Write("How many days is this subscription? (defualt: -1)");

input = Console.ReadLine();

model.daysLimit = Convert.ToInt32(input);

Console.Write("How many users is this subscription? (defualt: -1)");

input = Console.ReadLine();

model.deviceLimit = Convert.ToInt32(input);

Console.Write("How much traffic does this subscription have? (defualt: -1)");

input = Console.ReadLine();

model.trafficLimit = Convert.ToInt32(input);

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

Console.WriteLine("Are you sure ? (y|n)");

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
Console.Clear();

appConfig = new Config();

Console.WriteLine("Please enter server name");

input = Console.ReadLine();

appConfig.Name = input;

Console.WriteLine("Please enter V2ray port");

input = Console.ReadLine();

appConfig.Port = input;

Console.WriteLine("Please enter domain / hostname");
Console.WriteLine("note: if you have more than one domain / hostname seperate them with ',' with no space. ");

input = Console.ReadLine();

appConfig.HostNames = new List<string>();

foreach (var item in input.Split(","))
{
    appConfig.HostNames.Add(item.Trim());
}

Console.WriteLine("Please enter net");

input = Console.ReadLine();

appConfig.Net = input;

Console.WriteLine("Please enter type");

input = Console.ReadLine();

appConfig.Type = input;

Console.WriteLine("Please enter level");

input = Console.ReadLine();

appConfig.Level = input;

File.WriteAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath, JsonConvert.SerializeObject(appConfig));

Console.WriteLine("Config updated successfuly !\nPress a key to continue ...");

Console.ReadLine();

goto menu;


decide:
switch (Convert.ToInt32(input))
{
    case 0:
        return;

    case 1:
        goto getUser;

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
