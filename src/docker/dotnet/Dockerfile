FROM mdavid/centos:7

# Install .NET Core dependencies
RUN yum update -y \
    && yum install -y \
        epel-release \
        git-all \
        yum-utils
RUN rpm --import "http://keyserver.ubuntu.com/pks/lookup?op=get&search=0x3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF"
RUN yum-config-manager --add-repo http://download.mono-project.com/repo/centos/
RUN yum update -y
RUN yum -y install mono-complete


	    

