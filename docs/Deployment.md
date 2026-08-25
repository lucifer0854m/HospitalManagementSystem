# Deployment

## Docker Compose (recommended for local or single-host deployment)

1. Copy `.env.example` to `.env` and replace every password with a strong unique value.
2. Build and start the stack:

```powershell
.\scripts\deploy.ps1 -Build -Detach
```

3. Visit `http://localhost:8080/health/ready` to confirm the application and database are healthy, then sign in at `http://localhost:8080/Account/Login` with the configured initial administrator.

The first Compose startup applies the Entity Framework migration and creates the configured administrator. SQL Server data persists in the `sql-data` Docker volume.

## Existing databases

Set `Database__ApplyMigrations=false` when connecting to an existing database unless its migration history has been established and reviewed. Apply schema changes through the approved database-change process first.

## Backups

With the stack running and the Compose environment variables available, create a database backup:

```powershell
.\scripts\backup.ps1 -OutputPath .\backups\HospitalManagementDB.bak
```

Store backups outside the Docker host and verify restore procedures regularly.

## CI/CD

The GitHub Actions CI workflow restores, builds, and tests every pull request. A successful push to `main` also publishes the immutable commit image and `latest` image to GitHub Container Registry:

```text
ghcr.io/lucifer0854m/hospital-management-system:<commit-sha>
```

The manual **Deploy to Kubernetes** workflow deploys an image through the `production` GitHub environment. Before using it, add the base64 Kubernetes configuration to the `KUBE_CONFIG_DATA` repository secret and create the runtime secret from `k8s/secret.example.yaml` using your cloud secret manager or a protected CI command.

## Cloud deployment

The `k8s/` manifests run two application replicas behind a ClusterIP service, include CPU autoscaling, and use readiness/liveness probes. They work with managed Kubernetes services such as AKS, GKE, or EKS.

```powershell
kubectl apply -f k8s/configmap.yaml
# Create hospital-management-secrets securely; do not apply secret.example.yaml unchanged.
kubectl apply -f k8s/service.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/hpa.yaml
```

Use a managed SQL Server-compatible database for production. Set `Database__ApplyMigrations=false`; approve and apply migrations through your database-release process before deploying the application.

## Monitoring

Two health endpoints are available without authentication:

| Endpoint | Purpose |
| --- | --- |
| `/health/live` | Process liveness; used to restart unhealthy containers. |
| `/health/ready` | Process and database readiness; used before traffic reaches a pod. |

For local uptime monitoring, start the optional Uptime Kuma service and add a monitor for `http://web:8080/health/ready` from its dashboard:

```powershell
docker compose --env-file .env -f docker-compose.yml -f docker-compose.monitoring.yml --profile monitoring up -d
```

Open `http://localhost:3001` to configure notification channels. In cloud environments, point your platform monitor, load balancer, or external uptime provider at `/health/ready` and alert on failed checks, restart count, CPU utilization, memory utilization, and database availability.
