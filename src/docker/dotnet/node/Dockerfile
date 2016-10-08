FROM elastistack/centos-dotnet-core:7.1.1503-1.0.1

#Install official CentOS node distro
RUN yum install -y epel-release \
        && yum install -y nodejs

#Install latest node v4.x version supported by AWS Lambda
ENV NODE_VERSION 4.3.2
ENV NODE_VERSION_LINUX=node-v$NODE_VERSION-linux-x64
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/$NODE_VERSION_LINUX.tar.gz
ENV NODE_INSTALL_BIN_PATH /opt/node/$NODE_VERSION_LINUX/bin

RUN curl -sSL $NODE_DOWNLOAD_URL -o $NODE_VERSION_LINUX.tar.gz \
    && mkdir -p /opt/node \
    && tar -zxf $NODE_VERSION_LINUX.tar.gz -C /opt/node \
    && rm $NODE_VERSION_LINUX.tar.gz \
    && $NODE_INSTALL_BIN_PATH/npm install -g yo bower grunt-cli gulp generator-aspnet \
    && ln -s $NODE_INSTALL_BIN_PATH/* /usr/local/bin \
    && chmod g+rwx /root
