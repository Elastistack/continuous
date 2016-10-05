FROM elastistack/centos-dotnet-node:latest

RUN yum -y install unzip \
	&& curl "https://s3.amazonaws.com/aws-cli/awscli-bundle.zip" -o "awscli-bundle.zip" \
	&& unzip awscli-bundle.zip \
	&& ./awscli-bundle/install -i /usr/local/aws -b /usr/local/bin/aws \
	&& rm -f awscli-bundle.zip \
	&& rm -rf ./awscli-bundle

RUN yum install -y gcc-c++

#RUN npm install deepify -g

RUN npm install -g serverless \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/lib/node_modules/serverless/bin/serverless /usr/local/bin/sls \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/lib/node_modules/serverless/bin/serverless /usr/local/bin/slss \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/lib/node_modules/serverless/bin/serverless /usr/local/bin/serverless \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/bin/serverless-run-python-handler /usr/local/bin/serverless-run-python-handler


RUN npm install -g typescript \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/bin/tsc /usr/local/bin/tsc \
	&& ln -s /opt/node/node-v4.3.2-linux-x64/bin/tsserver /usr/local/bin/tsserver
