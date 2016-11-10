/*
The Lang Class adds easy to use multiple language support to any Unity project by parsing an XML file
containing numerous strings translated into any languages of your choice.  Refer to UMLS_Help.html and lang.xml
for more information.
Created by Adam T. Ryder
*/

import System.Xml;

static var instance : Lang;
static function GetInstance() : Lang
{
	if(instance == null)
	{
		var newObject = new GameObject();
		newObject.name = "Localisation Manager";
		instance = newObject.AddComponent.<Lang>();
		DontDestroyOnLoad(newObject);
	}
	return instance;
}
static function HasInstance()
{
	return instance != null;
}

private var Strings : Hashtable;
private var path : String = "lang";
private var language : String = "French";
private var texts : Array = new Array();
function Awake()
{
	Strings = new Hashtable();
	SetLanguage(path, language);
}
function SwitchLanguage(languageToSet : String)
{
	if(languageToSet == "French") language = "French";
	else if(languageToSet == "English") language = "English";
	SetLanguage (path, language);
}
function SetLanguage (path : String, language : String)
{
	var textData : TextAsset = Resources.Load(path, typeof(TextAsset));
	var xml : XmlDocument = new XmlDocument();
	xml.LoadXml(textData.text);

	Strings = new Hashtable();
	var element : XmlElement = xml.DocumentElement.Item[language];
	if (element)
	{
		var elemEnum : IEnumerator = element.GetEnumerator();
		while (elemEnum.MoveNext())
		{
			if(typeof(elemEnum.Current) == XmlElement)
			{
				var xmlItem : XmlElement = elemEnum.Current;
				Strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
			}
		}
	}
	else
		Debug.LogError("The specified language does not exist: " + language);
	Notify();
}
function GetString(name : String) : String
{
	if (!Strings.ContainsKey(name))
	{
		Debug.LogError("The specified string does not exist: " + name);
		return "";
	}

	return Strings[name];
}
function Register(text : LocalisedText)
{
	texts.push(text);
}
function Unregister(text : LocalisedText)
{
	texts.remove(text);
}
function Notify()
{
	for(var i = 0; i < texts.length; ++i)
	{
		var someText : LocalisedText = texts[i];
		someText.OnLanguageChange();
	}
}
