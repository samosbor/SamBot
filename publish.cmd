nuget restore
msbuild EchoBot.sln -p:DeployOnBuild=true -p:PublishProfile=testbotbuilder-Web-Deploy.pubxml -p:Password=G93EEXATaz2oAdnSjZpguCB4FqrD34u0h9Bnh3eMqPxRtWtdR52TnacdNSNr

