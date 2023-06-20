# nagp

#Git Code Path and commands for push/pull

#https://github.com/jaspreetsingh1989/nagp

#Check in code and yaml file commands
git init
git add *
git commit -m "nagp assignment"
git branch -M main
git remote add origin https://github.com/jaspreetsingh1989/nagp.git
git push -u origin main

#Checkout and pull commands
git clone https://github.com/jaspreetsingh1989/nagp.git


----------------------------------------------------------------------------------
----------------------------------------------------------------------------------
#Docker Image creation for Microservices solution and push to docker hub to make it publically available
#Open powershell (Run as administrator)
#Login to Docker account and create an image in the public repo already created there

#https://hub.docker.com/r/jaspreet2309/readdata/tags
#current v16 version is in use by microservice

docker login

#Navigate to dockerfile path present at root of the project(..code\Read.Data)

docker build -t jaspreet2309/readdata:v20 .

docker push jaspreet2309/readdata:v20

#If you want to pull the image created in dockerhub
docker pull jaspreet2309/readdata:v20



----------------------------------------------------------------------------------
----------------------------------------------------------------------------------
#Azure login, create resources and set subscription
#Navigate to https://portal.azure.com/ and login with the credentials
#Open cloudshell or use cmd and login

az login

az account set -s "<subscription-key>"

az group create --name nagprg --location "Central India"

az aks create -g nagprg -n aks-js-cluster1 --enable-managed-identity --node-count 1 --enable-addons monitoring --enable-msi-auth-for-monitoring  --generate-ssh-keys

az aks nodepool add --resource-group nagprg  --cluster-name aks-js-cluster1  --name nagpnodepool  --node-count 1 --node-vm-size Standard_B4ms --mode System --no-wait 
az aks nodepool delete --resource-group nagprg --name nodepool1 --cluster-name aks-js-cluster1 

az aks get-credentials -n "aks-js-cluster1" -g "nagprg"


----------------------------------------------------------------------------------
----------------------------------------------------------------------------------



#Kubernetes deployments
#Navigate to yaml files folder and run one by one

kubectl apply -f mysql-secret.yaml
kubectl apply -f configmap.yaml
kubectl apply -f mysql-storage.yaml
kubectl apply -f mysql-deployment.yaml
kubectl apply -f deployment.yaml
kubectl apply -f webapi-service.yaml

kubectl get pod
kubectl get services


------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
#MySQL database and table creations
#Copy the pod name of mysql pod after running kubectl get pods command and replace it by pod-name in below mentioned command

kubectl exec --stdin --tty "<mysql pod-name>" -- /bin/bash
mysql -p

#provide passowrd of root user as Root0++

#execute below mentioned commands for database creation and set-up

CREATE DATABASE Usersdb;
CREATE USER 'newuser'@'%' IDENTIFIED BY 'test1234';
UPDATE mysql.user SET Host='%' WHERE Host='localhost' AND User='newuser';
UPDATE mysql.db SET Host='%' WHERE Host='localhost' AND User='newuser';
FLUSH PRIVILEGES;
GRANT ALL PRIVILEGES ON Usersdb.* TO 'newuser'@'%';
GRANT ALL PRIVILEGES ON *.* TO 'newuser'@'%';
use Usersdb;
CREATE TABLE Users (    UserName VARCHAR(30) NOT NULL,    Hobbies VARCHAR(30) NOT NULL,    Location VARCHAR(50));
INSERT INTO Users (UserName,Hobbies,Location)
        VALUES ('UN1','HB1','LC1');
        INSERT INTO Users (UserName,Hobbies,Location)
        VALUES ('UN2','HB2','LC2');
        INSERT INTO Users (UserName,Hobbies,Location)
        VALUES ('UN3','HB3','LC3');
        INSERT INTO Users (UserName,Hobbies,Location)
        VALUES ('UN4','HB4','LC4');
        INSERT INTO Users (UserName,Hobbies,Location)
        VALUES ('UN5','HB5','LC5');";
		
		
-------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------

#microservice endpoints
http://20.102.32.142:8080/api/read-data