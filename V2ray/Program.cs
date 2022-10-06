
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using V2ray.Base;
using V2ray.Model;

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



menu:

Console.ForegroundColor = ConsoleColor.Blue;

Console.WriteLine("=================== Hello Welcome to V2ray Manager ===================");

Console.Write($"\n#####\t1. Update Config  \t2. Get Users\t#####");

Console.Write($"\n#####\t3. Create New User \t4. Delete User\t#####");

Console.Write($"\n#####\t5. Manual Settings \t6. Delete User\t#####\n");

input = Console.ReadLine();

goto decide;


updateConfig:
return;

getUsers:
return;

createNewUser:

Console.Write("Type UserName: ");

input = Console.ReadLine();

var model = new Client();
model.AlterId = 0;
model.CreateDate = DateTime.Now.ToString("yyyy-MM-DD");
model.Level = 0;
model.Id = Guid.NewGuid().ToString();
model.UserName = input;



var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(MainConstants.ConfgPath));

config?.inbounds[0]?.settings.Clients.Add(model);
//convert to json and add to clients VPN config

using (StreamWriter file = File.CreateText(MainConstants.ConfgPath))
{
    JsonSerializer serializer = new JsonSerializer();
    serializer.Serialize(file, config);
}


//process manager restart systemctl v2ray.service
ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "systemctl restart v2ray.service", };
Process proc = new Process() { StartInfo = startInfo, };
proc.Start();

var appConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath));

foreach (var hostName in appConfig.HostNames)
{//new user based on config

    var user = new User
    {
        Ps = appConfig.Name,
        Port = appConfig.Port,
        Net = appConfig.Net,
        Type = appConfig.Type,
        Host = hostName,
        Id = model.Id
    };
    var userJson = JsonConvert.SerializeObject(user);

    //print each one
    Console.WriteLine("\n"+ "vmess://"+Base64Encode(userJson));
}



return;


deleteUser:
return;

manualSettings:
return;


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