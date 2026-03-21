export function formatCurrency(
 value: number,
 locale: string,
 currency = 'VND',
 maximumFractionDigits = 0,
): string {
 return new Intl.NumberFormat(locale, {
  style: 'currency',
  currency,
  maximumFractionDigits,
 }).format(value);
}
