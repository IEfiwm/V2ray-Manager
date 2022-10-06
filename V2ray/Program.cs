
using Newtonsoft.Json;
using System.Text;
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
using Newtonsoft.Json;
using V2ray.Base;
using V2ray.Model;

string input = string.Empty;

if (!Directory.Exists(AppContext.BaseDirectory + MainConstants.FolderPath))
{
    Directory.CreateDirectory(AppContext.BaseDirectory + MainConstants.FolderPath);
}
try
{
    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(AppContext.BaseDirectory + MainConstants.AppConfigPath));
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

createnewuser:

Console.Write("Type UserName: ");

input = Console.ReadLine();

var model = new Client();
model.AlterId = 0;
model.CreateDate = DateTime.Now.ToString("yyyy-MM-DD");
model.Level = 0;
model.Id = Guid.NewGuid().ToString();
model.UserName = input;
return;
decide:
switch (Convert.ToInt32(input))
{
    case 1:
        break;

    case 2:
        break;

    case 3:
        goto createnewuser;
        break;

    case 4:
        break;

    case 5:
        break;

    case 6:
        break;

    default:
        break;
}


Console.ReadKey();