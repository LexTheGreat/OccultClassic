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
function onMenuLoad()
	util:cPrint("Loaded First Mod, 1.0.0")
end
hook:Add("onMenuLoad", "FirstMod_onMenuLoad", onMenuLoad)

function onGameLoad()
	util:cPrint("Init In-Game stuff here.")
	GameInput:Add("Space")
	GameInput["Space"]:Add(util:Key(57))
end
hook:Add("onGameLoad", "FirstMod_onGameLoad", onGameLoad)

function onUpdate()
	if GameInput["Space"]:IsPressed() then
		util:cPrint("We 'jumped'!")
	end
end
hook:Add("onUpdate", "FirstMod_onUpdate", onUpdate)