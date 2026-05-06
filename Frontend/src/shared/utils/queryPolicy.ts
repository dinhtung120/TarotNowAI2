import { useDebouncedValue } from '@/shared/hooks/useDebouncedValue';

interface ActionResultLike<T> {
 success: boolean;
 data?: T | null;
 error?: string;
}

export function queryFnOrThrow<T>(
 result: ActionResultLike<T>,
 fallbackMessage: string,
): T {
 if (!result.success || result.data == null) {
  throw new Error(result.error || fallbackMessage);
 }

 return result.data;
}

export function useDebouncedQueryInput<T>(value: T, delayMs: number): T {
 return useDebouncedValue(value, delayMs);
}

export interface CancellableLoadTask {
 cancel: () => void;
 createToken: () => number;
 isCurrentToken: (token: number) => boolean;
}

export function createCancellableLoadTask(): CancellableLoadTask {
 let currentToken = 0;

 return {
  createToken: () => {
   currentToken += 1;
   return currentToken;
  },
  isCurrentToken: (token) => token === currentToken,
  cancel: () => {
   currentToken += 1;
  },
 };
}
