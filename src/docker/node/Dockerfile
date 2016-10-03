FROM elastistack/centos:7

#Install official CentOS node distro
RUN yum install -y epel-release \
	&& yum install -y nodejs

#Install latest node v4.x version supported by AWS Lambda
ENV NODE_VERSION 4.3.2
ENV NODE_VERSION_LINUX=node-v$NODE_VERSION-linux-x64
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/$NODE_VERSION_LINUX.tar.gz

RUN curl -sSL $NODE_DOWNLOAD_URL -o $NODE_VERSION_LINUX.tar.gz \
    && mkdir -p /opt/node \
    && tar -zxf $NODE_VERSION_LINUX.tar.gz -C /opt/node \
    && rm $NODE_VERSION_LINUX.tar.gz \
    && ln -s /opt/node/$NODE_VERSION_LINUX/bin/* /usr/local/bin
