FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BlogApi/BlogApi.csproj", "BlogApi/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.Common/Blog.Common.csproj", "Blog.Common/"]
COPY ["Blog.Domain.Core/Blog.Domain.Core.csproj", "Blog.Domain.Core/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Infrastruct/Blog.Infrastruct.csproj", "Blog.Infrastruct/"]
COPY ["Blog.Dapper/Blog.Dapper.csproj", "Blog.Dapper/"]
COPY ["Blog.AOP/Blog.AOP.csproj", "Blog.AOP/"]
RUN dotnet restore "BlogApi/BlogApi.csproj"
COPY . .
WORKDIR "/src/BlogApi"
RUN dotnet build "BlogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogApi.dll"]