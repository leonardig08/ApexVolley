FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=myPassword1!

COPY attachdb.sh /usr/config/attachdb.sh
WORKDIR /usr/config

ENTRYPOINT ["bash", "/usr/config/attachdb.sh"]
