FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos .csproj e restaurar dependências
COPY ["src/LetCakes.API/LetCakes.API.csproj", "src/LetCakes.API/"]
COPY ["src/LetCakes.Application/LetCakes.Application.csproj", "src/LetCakes.Application/"]
COPY ["src/LetCakes.Domain/LetCakes.Domain.csproj", "src/LetCakes.Domain/"]
COPY ["src/LetCakes.Infrastructure/LetCakes.Infrastructure.csproj", "src/LetCakes.Infrastructure/"]

RUN dotnet restore "src/LetCakes.API/LetCakes.API.csproj"

# Copiar todo o código fonte
COPY . .

# Build e publish da aplicação
WORKDIR "/src/src/LetCakes.API"
RUN dotnet build "LetCakes.API.csproj" -c Release -o /app/build
RUN dotnet publish "LetCakes.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Criar usuário não-root para segurança
USER $APP_UID

# Copiar arquivos publicados do stage de build
COPY --from=build /app/publish .

# Expor porta 8080 (porta padrão para non-root)
EXPOSE 8080

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LetCakes.API.dll"]

