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

if (!File.Exists(MainConstants.ConfgPath))
{
    Console.Out.Flush();

    Console.ForegroundColor = ConsoleColor.Red;

    Console.Out.Flush();

    Console.Clear();

    Console.WriteLine("Error: V2ray not installed !");

    Console.WriteLine("\nPress any key to install V2ray ! ...");

    Console.ReadKey();

    installV2ray();

    installGeo();

    return;
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

    refreshSystem();
}

void refreshSystem()
{
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
}

void installV2ray()
{
    var startInfo = new ProcessStartInfo()
    {
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        FileName = "/bin/bash",
        Arguments = $"-c \"sudo bash <(curl -L https://raw.githubusercontent.com/v2fly/fhs-install-v2ray/master/install-release.sh)\""
    };

    Process proc = new Process() { StartInfo = startInfo, };
#if !DEBUG
proc.Start();
#endif
}

void installGeo()
{
    var startInfo = new ProcessStartInfo()
    {
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        FileName = "/bin/bash",
        Arguments = $"-c \"sudo bash <(curl -L https://raw.githubusercontent.com/v2fly/fhs-install-v2ray/master/install-dat-release.sh)\""
    };

    Process proc = new Process() { StartInfo = startInfo, };
#if !DEBUG
proc.Start();
#endif
}

void installBBR()
{
    var startInfo = new ProcessStartInfo()
    {
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        FileName = "/bin/bash",
        Arguments = $"-c \"sudo modprobe tcp_bbr;echo \"tcp_bbr\" >> /etc/modules-load.d/modules.conf;echo \"net.core.default_qdisc=fq\" >> /etc/sysctl.conf;echo \"net.ipv4.tcp_congestion_control=bbr\" >> /etc/sysctl.conf;sysctl -p\""
    };

    Process proc = new Process() { StartInfo = startInfo, };
#if !DEBUG
proc.Start();
#endif
}

menu:

foreach (var u in config.inbounds[0].settings.Clients
    .Where(m => m.daysLimit > 0 && DateTime.Parse(m.createDate).AddDays(m.daysLimit).Date < DateTime.Now.Date)
    .ToList())
{
    config.inbounds[0].settings.Clients.Remove(u);
}

saveToConfig();

Console.ForegroundColor = ConsoleColor.Blue;

Console.Out.Flush();

Console.Clear();

Console.WriteLine("=================== (Hello Welcome to V2ray Manager) ===================");

Console.WriteLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version);

Console.Write($"\n#####\t1. Get User  \t\t\t2. Get All Users\t#####");

Console.Write($"\n#####\t3. Create New User \t\t4. Delete User\t\t#####");

Console.Write($"\n#####\t5. Manual Settings \t\t6. Update Config\t#####");

Console.Write($"\n#####\t7. Create Backup \t\t8. Restore Backup\t#####");

Console.Write($"\n#####\t9. Install BBR \t\t\t10. Refresh System\t#####\n\n");

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

if (mmodel is null)
{
    Console.WriteLine("\nNo User found ! ...");

    Console.ReadKey();

    goto menu;
}

Console.WriteLine("========================================================");

Console.WriteLine($@"Id: {mmodel.id}");

Console.WriteLine($@"Username: {mmodel.username}");

Console.WriteLine($@"CreateDate: {mmodel.createDate}");

if (mmodel.daysLimit > 0)
    Console.WriteLine($@"ExpireDate: {DateTime.Parse(mmodel.createDate).AddDays(mmodel.daysLimit)}");

if (mmodel.deviceLimit > 0)
    Console.WriteLine($@"Device Limit: {mmodel.deviceLimit}");

if (mmodel.trafficLimit > 0)
    Console.WriteLine($@"Traffic Limit: {mmodel.trafficLimit}");

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
        V = appConfig.Level,
        Host = appConfig.Host,
        Path = appConfig.Path
    };
    var userJson = JsonConvert.SerializeObject(user, Formatting.Indented);

    //print each one
    Console.WriteLine($"\nvmess://{Base64Encode(userJson)}\n");
}

Console.WriteLine("\nPress a key to continue ...");

Console.ReadKey();

goto menu;

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

    if (item.daysLimit > 0)
        Console.WriteLine($@"ExpireDate: {DateTime.Parse(item.createDate).AddDays(item.daysLimit)}");

    if (item.deviceLimit > 0)
        Console.WriteLine($@"Device Limit: {item.deviceLimit}");

    if (item.trafficLimit > 0)
        Console.WriteLine($@"Traffic Limit: {item.trafficLimit}");

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
            V = appConfig.Level,
            Host = appConfig.Host,
            Path = appConfig.Path
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

var model = new Client();
model.alterId = 0;
model.createDate = DateTime.Now.ToString("yyyy-MM-dd");
model.level = Convert.ToInt16(appConfig.Level);

Console.Write("Please type Id: (defualt: NewUid)");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = Guid.NewGuid().ToString();
}

if ((bool)config.inbounds[0].settings.Clients.Any(m => m.id == input))
{
    Console.WriteLine("Error: Id is exist !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

if (!Guid.TryParse(input, out Guid res))
{
    Console.WriteLine("Error: Id is not valid !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

model.id = input;

Console.Write("Please type Username: (defualt: RndNumber)");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = DateTime.Now.Ticks.ToString();
}

model.username = input;

if ((bool)config.inbounds[0].settings.Clients.Any(m => m.username == input))
{
    Console.WriteLine("Error: Username is exist !\nPress a key to continue ...");

    Console.ReadKey();

    goto menu;
}

Console.Write("How many days is this subscription? (defualt: -1)");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = "-1";
}

model.daysLimit = Convert.ToInt32(input);

Console.Write("How many users is this subscription? (defualt: -1)");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = "-1";
}

model.deviceLimit = Convert.ToInt32(input);

Console.Write("How much traffic does this subscription have? (defualt: -1)");

input = Console.ReadLine();

if (input is null || string.IsNullOrEmpty(input))
{
    input = "-1";
}

model.trafficLimit = Convert.ToInt32(input);

config?.inbounds[0]?.settings.Clients.Add(model);
//convert to json and add to clients VPN config

saveToConfig();

Console.Clear();

Console.WriteLine("========================================================");

Console.WriteLine($@"Id: {model.id}");

Console.WriteLine($@"Username: {model.username}");

Console.WriteLine($@"CreateDate: {model.createDate}");

if (model.daysLimit > 0)
    Console.WriteLine($@"ExpireDate: {DateTime.Parse(model.createDate).AddDays(model.daysLimit)}");

if (model.deviceLimit > 0)
    Console.WriteLine($@"Device Limit: {model.deviceLimit}");

if (model.trafficLimit > 0)
    Console.WriteLine($@"Traffic Limit: {model.trafficLimit}");

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
        V = appConfig.Level,
        Host = appConfig.Host,
        Path = appConfig.Path
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

Console.WriteLine("Please enter security");

input = Console.ReadLine();

appConfig.Security = input;

Console.WriteLine("Please enter host");

input = Console.ReadLine();

appConfig.Host = input;

Console.WriteLine("Please enter path");

input = Console.ReadLine();

appConfig.Path = input;

File.WriteAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath, JsonConvert.SerializeObject(appConfig));

Console.WriteLine("Config updated successfuly !\nPress a key to continue ...");

Console.ReadLine();

goto menu;

updateConfig:
appConfig.Port = config.inbounds[0].port.ToString();
appConfig.Net = config.inbounds[0].streamSettings.network;
appConfig.Level = config.inbounds[0].settings.Clients.First().level.ToString();
Console.WriteLine("Config sync with V2ray Config successfuly !\nPress a key to continue ...");
Console.ReadLine();

goto menu;


decide:

if (string.IsNullOrEmpty(input))
    goto menu;

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
        goto updateConfig;

    case 7:
        goto createBackup;

    case 8:
        goto restoreBackup;

    case 9:
        goto installBBR;

    case 10:
        goto refreshSystem;

    default:
        goto decide;
}

createBackup:

Console.Clear();

Console.WriteLine("Are u sure ?! Do u want create backup point ? (default: n)");

input = Console.ReadLine();

if (input?.ToLowerInvariant() != "y")
{
    Console.WriteLine("Operation cancel by user !\nPress a key to continue ...");

    Console.ReadLine();

    goto menu;
}

if (File.Exists(AppContext.BaseDirectory + MainConstants.BackupPath))
    File.Delete(AppContext.BaseDirectory + MainConstants.BackupPath);

using (StreamWriter file = File.CreateText(AppContext.BaseDirectory + MainConstants.BackupPath))
{
    file.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
}

Console.WriteLine("Backup created !\nPress a key to continue ...");

Console.ReadLine();

goto menu;

restoreBackup:

Console.Clear();

Console.WriteLine("Are u sure ?! Do u want restore V2ray Config from backup point ? (default: n)");

input = Console.ReadLine();

if (input?.ToLowerInvariant() != "y")
{
    Console.WriteLine("Operation cancel by user !\nPress a key to continue ...");

    Console.ReadLine();

    goto menu;
}

if (!File.Exists(AppContext.BaseDirectory + MainConstants.BackupPath))
{
    Console.WriteLine("Error: Backup file not exist !\nPress a key to continue ...");

    Console.ReadLine();

    goto menu;
}

config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.BackupPath));

saveToConfig();

Console.WriteLine("Backup restored !\nPress a key to continue ...");

Console.ReadLine();

goto menu;

refreshSystem:

Console.Clear();

refreshSystem();

Console.WriteLine("System refreshed !\nPress a key to continue ...");

Console.ReadLine();

goto menu;

installBBR:

Console.Clear();

Console.WriteLine("BBR install successfuly !\nPress a key to continue ...");

Console.ReadLine();

installBBR();

goto menu;