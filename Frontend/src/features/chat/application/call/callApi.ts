import { apiUrl } from '@/shared/infrastructure/http/apiUrl';
import type { CallJoinTicketDto, CallSessionDto, CallType } from '@/features/chat/domain/callTypes';

export class CallApiError extends Error {
 code: string;
 status?: number;
 requestPath?: string;

 constructor(code: string, message: string, options?: { status?: number; requestPath?: string; cause?: unknown }) {
  super(message);
  this.name = 'CallApiError';
  this.code = code;
  this.status = options?.status;
  this.requestPath = options?.requestPath;
  if (options?.cause !== undefined) {
   (this as Error & { cause?: unknown }).cause = options.cause;
  }
 }
}

interface ProblemDetailsShape {
 title?: string;
 detail?: string;
 errorCode?: string;
 extensions?: { errorCode?: string };
}

async function callApi<T>(path: string, token: string, body?: object): Promise<T> {
 let response: Response;
 try {
  response = await fetch(apiUrl(path), {
   method: 'POST',
   headers: {
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + token,
   },
   body: body ? JSON.stringify(body) : undefined,
  });
 } catch (error) {
  throw new CallApiError('CALL_NETWORK_ERROR', 'Không thể kết nối máy chủ cuộc gọi.', {
   requestPath: path,
   cause: error,
  });
 }

 if (response.ok) {
  return response.json() as Promise<T>;
 }

 let problem: ProblemDetailsShape | null = null;
 try {
  problem = (await response.json()) as ProblemDetailsShape;
 } catch {
  problem = null;
 }

 const code = problem?.errorCode || problem?.extensions?.errorCode || 'CALL_API_ERROR';
 const message = problem?.detail || problem?.title || 'Call API request failed.';
 throw new CallApiError(code, message, {
  status: response.status,
  requestPath: path,
  cause: problem,
 });
}

export function startCallRequest(token: string, conversationId: string, type: CallType) {
 return callApi<CallJoinTicketDto>('/calls/start', token, { conversationId, type });
}

export function acceptCallRequest(token: string, callSessionId: string) {
 return callApi<CallJoinTicketDto>('/calls/' + encodeURIComponent(callSessionId) + '/accept', token);
}

export function endCallRequest(token: string, callSessionId: string, reason: string) {
 return callApi<CallSessionDto>('/calls/' + encodeURIComponent(callSessionId) + '/end', token, { reason });
}

export function issueCallTokenRequest(token: string, callSessionId: string) {
 return callApi<CallJoinTicketDto>('/calls/' + encodeURIComponent(callSessionId) + '/token', token);
}
