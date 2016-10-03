FROM mdavid/centos-dotnet:7-1.0.1-core-deps

# Install .NET Core
ENV DOTNET_VERSION 1.0.0
ENV DOTNET_SDK_VERSION preview2
ENV DOTNET_SDK_PATCH 003121
#ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/preview/Binaries/Latest/dotnet-centos-x64.latest.tar.gz
#ENV DOTNET_DOWNLOAD_URL https://download.microsoft.com/download/1/5/2/1523EBE1-3764-4328-8961-D1BD8ECA9295/dotnet-dev-centos-x64.$DOTNET_VERSION-$DOTNET_SDK_VERSION-$DOTNET_SDK_PATCH.tar.gz
ENV DOTNET_CORE_SDK_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/rel-1.0.0/dotnet-dev-centos-x64.latest.tar.gz

RUN curl -SL $DOTNET_CORE_SDK_URL --output dotnet.tar.gz \
    && mkdir -p /opt/dotnet \
    && tar -zxf dotnet.tar.gz -C /opt/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /opt/dotnet/dotnet /usr/bin/dotnet
