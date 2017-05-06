FROM microsoft/dotnet:1.1-sdk

# Restoring dependencies
COPY . .

ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR src/Budget.Api
RUN dotnet restore

EXPOSE 5000

# Running app
ENTRYPOINT ["dotnet", "run", "src/Budget.Api/Budget.Api.csproj"]
