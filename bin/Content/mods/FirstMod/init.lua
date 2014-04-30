--[[
-- UTIL
	util:cPrint(String Message, String Color)
	util:cPrint(String Message)


-- HOOK
	hook:Add(String hook, String name, Function)
	hook:Remove(String hook, String name)
	hook:Call(String hook)
	hook:Call(String hook, args)
]]-- 

-- Lets try hooks!
function onMenuLoad()
	util:cPrint("Loaded First Mod, 1.0.0")
	hook:Remove("onMenuLoad", "FirstMod_onMenuLoad")
end
hook:Add("onMenuLoad", "FirstMod_onMenuLoad", onMenuLoad)

function onGameLoad()
	util:cPrint("I load with the game :D")
	hook:Remove("onGameLoad", "FirstMod_onGameLoad")
end
hook:Add("onGameLoad", "FirstMod_onGameLoad", onGameLoad)

-- Hey lets add more hooks!
function onMenuLoad2()
	util:cPrint("Loaded First Mod, 1.0.0 AGAIN!")
	hook:Remove("onMenuLoad", "FirstMod_onMenuLoad2")
end
hook:Add("onMenuLoad", "FirstMod_onMenuLoad2", onMenuLoad2)

function onGameLoad2()
	util:cPrint("I load with the game :D AGAIN!")
	hook:Remove("onGameLoad", "FirstMod_onGameLoad2")
end
hook:Add("onGameLoad", "FirstMod_onGameLoad2", onGameLoad2)'


--[[
cPrint is print with color. Using cPrint also adds a time stamp. Would use over regular print!
cPrint("Text", Optional "Color")

Hooks are added by hookname, custom name, and function (Without ()'s)
hook:Add("hookname", "custom name", function)

Hooks are then removed by hookname, custom name
hook:Remove("hookname", custom name", function)

An added hook is called by the variable hook,
hook:Call("hookName", optional arguments)

--]]