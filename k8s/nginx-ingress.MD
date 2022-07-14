

### The following link is for the help https://kubernetes.github.io/ingress-nginx/deploy/#quick-start

# apply the ingress rules to the pods
````kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.3.0/deploy/static/provider/cloud/deploy.yaml```

```kubectl get pods --namespace=ingress-nginx````