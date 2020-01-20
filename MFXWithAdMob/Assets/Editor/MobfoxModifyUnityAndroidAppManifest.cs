using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;
using GoogleMobileAds.Editor;

using System.Linq;
using System.Xml.Linq;

public class MobfoxModifyUnityAndroidAppManifest : IPostGenerateGradleAndroidProject
{
    private const string META_AD_MANAGER_APP = "com.google.android.gms.ads.AD_MANAGER_APP";

    private const string META_APPLICATION_ID  = "com.google.android.gms.ads.APPLICATION_ID";

    private const string META_DELAY_APP_MEASUREMENT_INIT =
            "com.google.android.gms.ads.DELAY_APP_MEASUREMENT_INIT";

    private XNamespace ns = "http://schemas.android.com/apk/res/android";

 
    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        // If needed, add condition checks on whether you need to run the modification routine.
        // For example, specific configuration/app options enabled

        var androidManifest = new AndroidManifest(GetManifestPath(basePath));

        // Add your XML manipulation routines

        androidManifest.AddPermission("android.permission.ACCESS_WIFI_STATE");
        androidManifest.AddPermission("android.permission.CHANGE_WIFI_STATE");
        androidManifest.AddPermission("android.permission.ACCESS_COARSE_LOCATION");
        androidManifest.AddPermission("android.permission.ACCESS_NETWORK_STATE");
        androidManifest.AddPermission("android.permission.INTERNET");
        
        androidManifest.AddInterstialActivity();
        androidManifest.AddBrowserActivity();
        androidManifest.AddService();
        
        androidManifest.AddClearTextSupport();
//@@@        androidManifest.AddNetworkSecurityConfig();
        
        androidManifest.SetHardwareAccel();

        androidManifest.TurnAndroidXOn(basePath);

        androidManifest.Save();
    }

    public int callbackOrder { get { return 1; } }

    private string _manifestFilePath;

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(_manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();
        }
        return _manifestFilePath;
    }
}


internal class AndroidXmlDocument : XmlDocument
{
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public AndroidXmlDocument(string path)
    {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path))
        {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save()
    {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}


internal class AndroidManifest : AndroidXmlDocument
{
    private readonly XmlElement ApplicationElement;

    public AndroidManifest(string path) : base(path)
    {
        ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
    }

    internal XmlNode GetActivityWithLaunchIntent()
    {
        return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value)
    {
        XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }
    
    internal void AddInterstialActivity()
    {
        XmlElement child = CreateElement("activity");
        ApplicationElement.AppendChild(child);

        XmlAttribute newAttribute1 = CreateAndroidAttribute("name", "com.mobfox.android.Ads.InterstitialActivity");
        child.Attributes.Append(newAttribute1);

        XmlAttribute newAttribute2 = CreateAndroidAttribute("hardwareAccelerated", "true");
        child.Attributes.Append(newAttribute2);

        XmlAttribute newAttribute3 = CreateAndroidAttribute("configChanges", "orientation|screenSize");
        child.Attributes.Append(newAttribute3);

        XmlAttribute newAttribute4 = CreateAndroidAttribute("theme", "@android:style/Theme.NoTitleBar.Fullscreen");
        child.Attributes.Append(newAttribute4);
    }
    
    internal void AddBrowserActivity()
    {
        XmlElement child = CreateElement("activity");
        ApplicationElement.AppendChild(child);

        XmlAttribute newAttribute1 = CreateAndroidAttribute("name", "com.mobfox.android.core.InAppBrowser");
        child.Attributes.Append(newAttribute1);

        XmlAttribute newAttribute2 = CreateAndroidAttribute("hardwareAccelerated", "true");
        child.Attributes.Append(newAttribute2);

        XmlAttribute newAttribute3 = CreateAndroidAttribute("configChanges", "orientation|screenSize");
        child.Attributes.Append(newAttribute3);
    }
    
    internal void AddService()
    {
        XmlElement child = CreateElement("service");
        ApplicationElement.AppendChild(child);

        XmlAttribute newAttribute1 = CreateAndroidAttribute("name", "com.mobfox.android.dmp.services.MobFoxService");
        child.Attributes.Append(newAttribute1);

        XmlAttribute newAttribute2 = CreateAndroidAttribute("launchMode", "singleTop");
        child.Attributes.Append(newAttribute2);
    }

    internal void AddPermission(string permissionName)
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", permissionName);
        child.Attributes.Append(newAttribute);
    }
    
    internal void AddClearTextSupport()
    {
        XmlAttribute newAttribute = CreateAndroidAttribute("usesCleartextTraffic", "true");
        ApplicationElement.Attributes.Append(newAttribute);
    }
    
    internal void AddNetworkSecurityConfig()
    {
        XmlAttribute newAttribute = CreateAndroidAttribute("networkSecurityConfig", "@xml/network_security_config");
        ApplicationElement.Attributes.Append(newAttribute);
    }
    
    internal void SetHardwareAccel()
    {
   		GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true")); 
	} 
	
	internal void TurnAndroidXOn(string path)
	{
	    string gradlePropertiesFile = path + "/gradle.properties";
        if (File.Exists(gradlePropertiesFile))
        {
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
        writer.WriteLine("android.useAndroidX=true");
        writer.WriteLine("android.enableJetifier=true");
        writer.Flush();
        writer.Close();
	}
}
