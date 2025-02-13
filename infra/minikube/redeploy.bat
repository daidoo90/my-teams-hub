@echo off

:: Ensure Minikube is running
minikube start

:: Build the Docker image
docker build -t localhost:5000/my-teams-hub-api:latest -f src\MyTeamsHub.Organization.API\Dockerfile .

:: Push the Docker image to the local registry
docker push localhost:5000/my-teams-hub-api:latest

:: Organization API ===============================================================================
kubectl apply -f infra\organization-api\deployment.yaml
kubectl apply -f infra\organization-api\service.yaml

:: MSSQL ===============================================================================
kubectl apply -f infra\mssql\deployment.yaml
kubectl apply -f infra\mssql\service.yaml

:: Trigger a rollout restart to update the service with the new image
kubectl rollout restart deployment/my-teams-hub-api-deployment

:: Open the Minikube dashboard (optional)
minikube dashboard

:: Start local docker registry
:: docker run -d -p 5000:5000 --name registry registry:2

:: Minikube running and access to local machine
::minikube tunnel


::kubectl create secret docker-registry my-registry-secret --docker-server=localhost:5000 --docker-username=myusername --docker-password=mypassword --docker-email=myemail@example.com


::curl http://localhost:5000/v2/my-teams-hub-api/manifests/latest