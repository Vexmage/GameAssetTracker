# Step 1: Use the official .NET SDK to build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and build the project
COPY . ./
RUN dotnet publish -c Release -o /out

# Step 2: Use NGINX to serve the Blazor WASM app
FROM nginx:alpine
COPY --from=build /out/wwwroot /usr/share/nginx/html

# Expose port 80 for HTTP traffic
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]
