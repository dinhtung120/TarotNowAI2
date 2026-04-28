export function resolveErrorMessage(error: unknown, fallbackMessage: string): string {
 if (error instanceof Error && error.message.trim()) {
  return error.message;
 }

 return fallbackMessage;
}
