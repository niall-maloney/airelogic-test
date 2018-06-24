FROM microsoft/mssql-server-linux:2017-latest 
ENV SA_PASSWORD=Passw0rd
ENV ACCEPT_EULA=Y
COPY entrypoint.sh entrypoint.sh
COPY startup.sh startup.sh
COPY build.sql build.sql
RUN chmod +x ./startup.sh
CMD /bin/bash ./entrypoint.sh