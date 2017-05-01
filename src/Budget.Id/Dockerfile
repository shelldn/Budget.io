FROM microsoft/dotnet:1.1-sdk

# Restoring dependencies
COPY . .

WORKDIR src/Budget.Id
RUN dotnet restore

EXPOSE 5000

# Running app
ENTRYPOINT ["dotnet", "run", "src/Budget.Id/Budget.Id.csproj"]
