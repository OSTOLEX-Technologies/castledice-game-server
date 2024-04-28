FROM --platform=linux/arm64 130936542519.dkr.ecr.eu-central-1.amazonaws.com/castledice-riptide_server:latest

ARG NET_VERSION=7.0.14
ENV ASPNET_VERSION=$NET_VERSION

RUN apk --no-cache add jq

COPY . /app/
RUN cp /usr/share/dotnet/shared/Microsoft.NETCore.App/$NET_VERSION/*.so /app/

WORKDIR /app

RUN chmod +x entrypoint.sh

EXPOSE 7779

CMD ["sh", "/app/entrypoint.sh"]
