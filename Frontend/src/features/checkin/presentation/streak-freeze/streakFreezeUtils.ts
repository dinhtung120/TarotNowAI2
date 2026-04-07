export function getErrorDescription(error: unknown): string {
  if (error instanceof Error && error.message) return error.message;
  if (typeof error === 'object' && error !== null) {
    const maybeHttpError = error as { response?: { data?: { message?: string } } };
    if (maybeHttpError.response?.data?.message) return maybeHttpError.response.data.message;
  }
  return 'Có lỗi khi mua quyền cứu vớt.';
}

export function getRemainingTimeParts(remainingSeconds: number) {
  return {
    hours: Math.floor(remainingSeconds / 3600),
    minutes: Math.floor((remainingSeconds % 3600) / 60),
  };
}
