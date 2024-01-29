FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app


COPY ./BE ./
RUN dotnet restore

RUN dotnet build -c Release -o out
RUN dotnet dev-certs https -ep /https/httpscert.pfx -p pass

FROM node:20 AS build-env-FE
WORKDIR /app

COPY ./FE ./
RUN npm install
RUN npm run lint
RUN npm run build



# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env-FE /app/dist ./App
COPY --from=build-env /https /https
ENV ASPNETCORE_URLS=https://+;http://+
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=pass
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/httpscert.pfx

# Add wait-for-it
COPY wait-for-it.sh wait-for-it.sh
RUN chmod +x wait-for-it.sh

#ENTRYPOINT [ "/bin/bash", "-c" ]
#CMD ["/wait-for-it.sh", "database:1433", "-t", "120", "--", "dotnet", "Artin.BringAuto.dll"]

ENTRYPOINT ["dotnet", "Artin.BringAuto.dll"]
