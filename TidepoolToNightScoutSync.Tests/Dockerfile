FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TidepoolToNightScoutSync.Tests/TidepoolToNightScoutSync.Tests.csproj", "TidepoolToNightScoutSync.Tests/"]
COPY ["TidepoolToNightScoutSync.BL/TidepoolToNightScoutSync.BL.csproj", "TidepoolToNightScoutSync.BL/"]
RUN dotnet restore "TidepoolToNightScoutSync.Tests/TidepoolToNightScoutSync.Tests.csproj"
COPY . .
WORKDIR "/src/TidepoolToNightScoutSync.Tests"
RUN dotnet build "TidepoolToNightScoutSync.Tests.csproj"

## Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
RUN chmod +x /wait

## Launch the wait tool and then your application
CMD /wait && dotnet test TidepoolToNightScoutSync.Tests.csproj
# ENTRYPOINT ["dotnet" ,"test", "TidepoolToNightScoutSync.Tests.csproj"]