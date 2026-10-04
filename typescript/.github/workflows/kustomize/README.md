# Kubernetes deployment (Kustomize)

`deploy-typescript.yml` builds the Docker image of this project, pushes it to GitHub Container Registry and deploys it with one of these overlays:

| Folder  | Namespace    | Replicas | Deployed by |
|---------|--------------|----------|-------------|
| `dev`   | `toll-dev`   | 1        | every push to `main` (except changes to Markdown files) |
| `stage` | `toll-stage` | 2        | manual run of the workflow (environment `stage`) |
| `prod`  | `toll-prod`  | 3        | manual run of the workflow (environment `prod`) |

`base/` holds the Deployment and Service shared by all three. Each overlay adds its own namespace, image, replica count and the two env files below.

## Environment variables

| File          | Becomes | Contents |
|---------------|---------|----------|
| `<env>/app.env`     | ConfigMap `toll-service-config` | `MONGODB_DB`, `TOLL_FEE_LOW`, `TOLL_FEE_MEDIUM`, `TOLL_FEE_HIGH`, `TOLL_MAX_DAILY_FEE` (placeholder values, not secret) |
| `<env>/secrets.env` | Secret `toll-service-secrets` | `MONGODB_URI`, `MONGODB_USERNAME`, `MONGODB_PASSWORD` (placeholders; the workflow overwrites them) |

Both are injected into the container as environment variables. `PORT` is set to 3000 in `base/deployment.yaml`. A change to either file gives the ConfigMap or Secret a new name, so the pods restart on the next deploy.

## One-time setup

1. In GitHub, create the environments `dev`, `stage` and `prod` (Settings, Environments). Adding required reviewers to `stage` and `prod` makes those deploys wait for approval.
2. In each environment add these secrets:
   - `KUBE_CONFIG`: a kubeconfig for that cluster, base64 encoded (`base64 -w0 kubeconfig` on Linux, `base64 -i kubeconfig` on macOS).
   - `MONGODB_URI`: the connection string of an existing MongoDB. These manifests don't deploy MongoDB, and the app won't start without it.
   - `MONGODB_USERNAME` and `MONGODB_PASSWORD`: the database credentials, kept out of the URI. For a MongoDB without authentication, set them to empty values.
3. Replace the placeholder values in each `app.env`, and the namespaces and replica counts if you want others.
4. If the package is private, the cluster needs an image pull secret for `ghcr.io`.

## Promoting an image

Run the workflow manually, pick `stage` or `prod`, and put an image tag from an earlier build (the commit SHA) in `image_tag`. The image isn't rebuilt.

## Rendering locally

```bash
kustomize build .github/workflows/kustomize/dev
```

The output includes the Secret, base64 encoded, so don't paste it anywhere shared.
