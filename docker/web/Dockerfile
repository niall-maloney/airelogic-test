# Build image
FROM microsoft/aspnetcore-build:2.0-stretch AS build-env  

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./AireLogic.sln  ./
COPY src/AireLogic/AireLogic.csproj  ./src/AireLogic/AireLogic.csproj
RUN dotnet restore

# Copy everything else and build
COPY src ./src
RUN dotnet publish ./src/AireLogic/AireLogic.csproj -c Release -o ../../out

LABEL maintainer="niall-maloney"

# Build runtime image
FROM microsoft/aspnetcore:2.0-stretch
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "AireLogic.dll"]