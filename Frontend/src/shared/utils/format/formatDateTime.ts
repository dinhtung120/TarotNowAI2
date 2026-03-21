export function formatDate(value: string | Date, locale: string): string {
 const date = typeof value === 'string' ? new Date(value) : value;
 return date.toLocaleDateString(locale);
}

export function formatTime(
 value: string | Date,
 locale: string,
 options?: Intl.DateTimeFormatOptions,
): string {
 const date = typeof value === 'string' ? new Date(value) : value;
 return date.toLocaleTimeString(locale, options);
}

export function formatDateTime(value: string | Date, locale: string): string {
 const date = typeof value === 'string' ? new Date(value) : value;
 return date.toLocaleString(locale);
}
