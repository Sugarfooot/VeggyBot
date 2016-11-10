#pragma strict
#pragma downcast

import UnityEngine.UI;

private var textObject : Text;
private var textIdentifier = null;
public var allCaps = false;

function Start ()
{
	textObject = GetComponent.<Text>();
	if(textIdentifier == null)
		textIdentifier = textObject.text;
	OnLanguageChange();
	Lang.GetInstance().Register(this);
}

function OnDestroy()
{
	if(Lang.HasInstance())
		Lang.GetInstance().Unregister(this);
}

function OnLanguageChange()
{
	if(textIdentifier != "" && textObject != null)
	{
		if(allCaps)
			textObject.text = Lang.GetInstance().GetString(textIdentifier).ToUpper();
		else
			textObject.text = Lang.GetInstance().GetString(textIdentifier);
	}
}

function setNewText(text : String)
{
	textIdentifier = text;
	OnLanguageChange();
}