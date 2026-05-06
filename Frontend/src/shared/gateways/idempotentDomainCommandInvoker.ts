'use server';

import { randomUUID } from 'node:crypto';
import { AUTH_HEADER } from '@/shared/gateways/authConstants';
import {
 invokeDomainCommand,
 type DomainCommandKey,
} from '@/shared/gateways/domainCommandRegistry';
import type {
 ServerHttpResult,
} from '@/shared/gateways/serverHttpClient';

type JsonPayload = Record<string, unknown>;

interface IdempotentDomainCommandOptions<TPayload extends JsonPayload> {
 path: string;
 method: 'POST' | 'PUT' | 'PATCH' | 'DELETE';
 token: string;
 payload: TPayload;
 headers?: HeadersInit;
 fallbackErrorMessage: string;
}

interface IdempotentPayload extends JsonPayload {
 idempotencyKey: string;
}

function resolveIdempotencyKey(payload: JsonPayload): string {
 const candidate = payload.idempotencyKey;
 if (typeof candidate === 'string' && candidate.trim().length > 0) {
  return candidate.trim();
 }

 return randomUUID();
}

export async function createIdempotentDomainCommandInvoker<
 TResponse,
 TPayload extends JsonPayload,
>(
 key: DomainCommandKey,
 options: IdempotentDomainCommandOptions<TPayload>,
): Promise<ServerHttpResult<TResponse>> {
 const idempotencyKey = resolveIdempotencyKey(options.payload);
 const mergedHeaders = new Headers(options.headers);
 mergedHeaders.set(AUTH_HEADER.IDEMPOTENCY_KEY, idempotencyKey);

 const payload: IdempotentPayload = {
  ...options.payload,
  idempotencyKey,
 };

 return invokeDomainCommand<TResponse>(key, {
  path: options.path,
  method: options.method,
  token: options.token,
  headers: Object.fromEntries(mergedHeaders.entries()),
  json: payload,
  fallbackErrorMessage: options.fallbackErrorMessage,
 });
}
