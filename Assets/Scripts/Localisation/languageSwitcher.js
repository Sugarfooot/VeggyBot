#pragma strict

function switchLanguage(language : String)
{
	Lang.GetInstance().SwitchLanguage(language);
}