

deploy:
	dotnet build
	mkdir -p ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter"
	cp bin/Debug/net471/fermenter.dll ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter/" -v
	cp bin/Debug/net471/CaiLib.dll ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter/" -v
	cp mod_info.yaml ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter/" -v
	cp mod.yaml ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter/" -v
	cp -R anim ~/".config/unity3d/Klei/Oxygen Not Included/mods/Dev/Fermenter/" -v