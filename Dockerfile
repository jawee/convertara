FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY ./example App/
COPY ./src/lib App/
COPY ./output App/
COPY ./src/bin/Debug/net5.0/ App/src/
WORKDIR /App
ENTRYPOINT ["dotnet", "src/dotnet-ffmpeg-console.dll"]

