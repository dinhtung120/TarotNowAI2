import { EVENT_CONTRACTS } from '@/shared/domain/eventContracts';
import { logger } from '@/shared/application/gateways/logger';
import {
 serverHttpRequest,
 type ServerHttpRequestOptions,
 type ServerHttpResult,
} from '@/shared/application/gateways/serverHttpClient';

type DomainCommandMethod = 'POST' | 'PUT' | 'PATCH' | 'DELETE';
type DomainCommandOwnership = 'wallet' | 'reading' | 'chat' | 'admin';

export interface DomainCommandContract {
 method: DomainCommandMethod;
 pathPattern: RegExp;
 expectedEvents: readonly string[];
 ownership: DomainCommandOwnership;
}

const DOMAIN_COMMAND_REGISTRY = {
 'wallet.withdrawal.admin.process': {
  method: 'POST',
  pathPattern: /^\/admin\/withdrawals\/process$/,
  expectedEvents: EVENT_CONTRACTS.walletWithdrawal,
  ownership: 'wallet',
 },
 'admin.user.adjust-balance': {
  method: 'PUT',
  pathPattern: /^\/admin\/users\/[^/]+$/,
  expectedEvents: EVENT_CONTRACTS.adminUserBalanceAdjust,
  ownership: 'admin',
 },
 'chat.dispute.resolve': {
  method: 'POST',
  pathPattern: /^\/admin\/disputes\/[^/]+\/resolve$/,
  expectedEvents: EVENT_CONTRACTS.chatDisputeResolve,
  ownership: 'chat',
 },
 'reading.session.init': {
  method: 'POST',
  pathPattern: /^\/reading\/init$/,
  expectedEvents: EVENT_CONTRACTS.readingInit,
  ownership: 'reading',
 },
 'reading.session.reveal': {
  method: 'POST',
  pathPattern: /^\/reading\/reveal$/,
  expectedEvents: EVENT_CONTRACTS.readingReveal,
  ownership: 'reading',
 },
 'wallet.withdrawal.reader.create': {
  method: 'POST',
  pathPattern: /^\/withdrawal\/create$/,
  expectedEvents: EVENT_CONTRACTS.walletWithdrawal,
  ownership: 'wallet',
 },
 'wallet.deposit.create': {
  method: 'POST',
  pathPattern: /^\/deposits\/orders$/,
  expectedEvents: EVENT_CONTRACTS.walletDeposit,
  ownership: 'wallet',
 },
 'wallet.deposit.reconcile': {
  method: 'POST',
  pathPattern: /^\/deposits\/orders\/[^/]+\/reconcile$/,
  expectedEvents: EVENT_CONTRACTS.walletDeposit,
  ownership: 'wallet',
 },
 'wallet.deposit.admin.process': {
  method: 'PATCH',
  pathPattern: /^\/admin\/deposits\/process$/,
  expectedEvents: EVENT_CONTRACTS.walletDeposit,
  ownership: 'wallet',
 },
 'admin.reader-request.process': {
  method: 'PATCH',
  pathPattern: /^\/admin\/reader-requests\/process$/,
  expectedEvents: EVENT_CONTRACTS.adminReaderRequestProcess,
  ownership: 'admin',
 },
 'chat.add-money.request': {
  method: 'POST',
  pathPattern: /^\/conversations\/[^/]+\/add-money\/request$/,
  expectedEvents: EVENT_CONTRACTS.chatAddMoneyRequest,
  ownership: 'chat',
 },
 'chat.add-money.respond': {
  method: 'POST',
  pathPattern: /^\/conversations\/[^/]+\/add-money\/respond$/,
  expectedEvents: EVENT_CONTRACTS.chatAddMoneyRespond,
  ownership: 'chat',
 },
 'chat.dispute.open': {
  method: 'POST',
  pathPattern: /^\/conversations\/[^/]+\/dispute$/,
  expectedEvents: EVENT_CONTRACTS.chatDisputeOpen,
  ownership: 'chat',
 },
} as const satisfies Record<string, DomainCommandContract>;

export type DomainCommandKey = keyof typeof DOMAIN_COMMAND_REGISTRY;

interface InvokeDomainCommandOptions extends Omit<ServerHttpRequestOptions, 'method' | 'expectedDomainEvents'> {
 path: string;
 method?: DomainCommandMethod;
}

interface DomainCommandValidationFailure {
 error: string;
 meta: Record<string, unknown>;
}

function normalizeMethod(value: string | undefined, fallback: DomainCommandMethod): DomainCommandMethod {
 if (!value) {
  return fallback;
 }

 const normalized = value.toUpperCase();
 if (normalized === 'POST' || normalized === 'PUT' || normalized === 'PATCH' || normalized === 'DELETE') {
  return normalized;
 }

 return fallback;
}

function validateCommandInvocation(
 key: DomainCommandKey,
 contract: DomainCommandContract,
 path: string,
 method: DomainCommandMethod,
): DomainCommandValidationFailure | null {
 if (method !== contract.method) {
  return {
   error: 'domain-command.method-mismatch',
   meta: {
    key,
    expectedMethod: contract.method,
    receivedMethod: method,
    path,
   },
  };
 }

 if (!contract.pathPattern.test(path)) {
  return {
   error: 'domain-command.path-mismatch',
   meta: {
    key,
    method,
    path,
    expectedPattern: contract.pathPattern.source,
   },
  };
 }

 return null;
}

function toFailedCommandResult(message: string): ServerHttpResult<never> {
 return {
  ok: false,
  status: 500,
  headers: new Headers(),
  error: message,
 };
}

export function getDomainCommandContract(key: DomainCommandKey): DomainCommandContract {
 return DOMAIN_COMMAND_REGISTRY[key];
}

export function getDomainCommandRegistry(): Record<DomainCommandKey, DomainCommandContract> {
 return DOMAIN_COMMAND_REGISTRY;
}

export async function invokeDomainCommand<T>(
 key: DomainCommandKey,
 options: InvokeDomainCommandOptions,
): Promise<ServerHttpResult<T>> {
 const contract = getDomainCommandContract(key);
 const method = normalizeMethod(options.method, contract.method);
 const validationFailure = validateCommandInvocation(key, contract, options.path, method);
 if (validationFailure) {
  logger.error('[DomainCommand]', 'domain-command.missing-contract', validationFailure.meta);
  return toFailedCommandResult(validationFailure.error);
 }

 const { path, ...rest } = options;
 return serverHttpRequest<T>(path, {
  ...rest,
  method,
  expectedDomainEvents: contract.expectedEvents,
 });
}
