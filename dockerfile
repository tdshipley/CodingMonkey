FROM microsoft/aspnetcore-build:2.0.3 AS builder
# Stage 1
    WORKDIR /source

    # caches restore result by copying csproj file separately
    COPY ./src/CodingMonkey/*.csproj .
    RUN dotnet restore

    # copies the rest of your code
    COPY ./src/CodingMonkey/ .
    RUN dotnet publish --output /app/ --configuration Release

# Stage 2
    FROM microsoft/aspnetcore
    WORKDIR /app
    COPY --from=builder /app .
    # Set port for Heroku
    ENV ASPNETCORE_URLS http://*:$PORT
    CMD ["echo $ASPNETCORE_URLS", "dotnet", "CodingMonkey.dll"]