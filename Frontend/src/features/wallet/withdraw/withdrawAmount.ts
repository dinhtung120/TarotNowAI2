const WHOLE_NUMBER_PATTERN = /^\d+$/;

export function parseWholeWithdrawAmount(value: string): number | null {
 const normalized = value.trim();
 if (!WHOLE_NUMBER_PATTERN.test(normalized)) {
  return null;
 }

 return Number(normalized);
}
