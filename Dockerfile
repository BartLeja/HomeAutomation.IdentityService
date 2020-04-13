#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 51428
EXPOSE 44342

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["HomeAutomation.IdentityService/HomeAutomation.IdentityService.csproj", "HomeAutomation.IdentityService/"]
RUN dotnet restore "HomeAutomation.IdentityService/HomeAutomation.IdentityService.csproj"
COPY . .
WORKDIR "/src/HomeAutomation.IdentityService"
RUN dotnet build "HomeAutomation.IdentityService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HomeAutomation.IdentityService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HomeAutomation.IdentityService.dll"]