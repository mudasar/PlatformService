# Some commands for kubectl

* restart the deployment 
kubectl rollout restart deployment <deployment_name> -n <namespace>

kubectl delete replicaset <name> -n <namespace>

kubectl get pod <pod_name> -n <namespace> -o yaml | kubectl replace --force -f -


-- default rabbitmq management console login 
username: guest
password: guest


create a hosts file entry with domain name platforms.io.local 