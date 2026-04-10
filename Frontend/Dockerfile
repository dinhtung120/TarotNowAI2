# syntax=docker/dockerfile:1.7

FROM node:22.15.1-bookworm-slim AS deps
WORKDIR /app
COPY Frontend/package.json Frontend/package-lock.json ./
RUN npm ci

FROM node:22.15.1-bookworm-slim AS builder
WORKDIR /app
ENV NEXT_TELEMETRY_DISABLED=1
ARG NEXT_PUBLIC_API_URL
ARG NEXT_PUBLIC_BASE_URL
ARG NEXTAUTH_URL
ENV NEXT_PUBLIC_API_URL=${NEXT_PUBLIC_API_URL}
ENV NEXT_PUBLIC_BASE_URL=${NEXT_PUBLIC_BASE_URL}
ENV NEXTAUTH_URL=${NEXTAUTH_URL}
COPY --from=deps /app/node_modules ./node_modules
COPY Frontend/ ./
RUN npm run build

FROM node:22.15.1-bookworm-slim AS runner
WORKDIR /app

ENV NODE_ENV=production
ENV NEXT_TELEMETRY_DISABLED=1
ENV HOSTNAME=0.0.0.0
ENV PORT=3000

RUN apt-get update \
    && apt-get install -y --no-install-recommends wget ca-certificates \
    && rm -rf /var/lib/apt/lists/*

RUN if ! getent group app > /dev/null; then groupadd --gid 10001 app; fi \
    && if ! id -u app > /dev/null 2>&1; then useradd --uid 10001 --gid app --create-home --home-dir /home/app --shell /usr/sbin/nologin app; fi

COPY --from=builder /app/public ./public
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

USER app

EXPOSE 3000

CMD ["node", "server.js"]
